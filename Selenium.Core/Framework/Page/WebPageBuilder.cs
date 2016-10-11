namespace Selenium.Core.Framework.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using NUnit.Framework;

    using Selenium.Core.Framework.PageElements;

    public static class WebPageBuilder
    {
        public static void InitPage(IPage page)
        {
            InitComponents(page, page);
        }

        /// <summary>
        ///     ������� ������ ��������� � ��� ������� �������� ���������������� ��� ��������� ����������
        ///     (���������� ��������� WebComponent)
        /// </summary>
        public static List<T> CreateItems<T>(IContainer container, IEnumerable<string> ids)
        {
            return ids.Select(id => CreateComponent<T>(container.ParentPage, container, id)).Cast<T>().ToList();
        }

        public static IComponent CreateComponent<T>(IContainer container, params object[] additionalArgs)
        {
            var component = CreateComponent(
                container.ParentPage,
                container,
                typeof(T),
                new WebComponentAttribute(additionalArgs));
            InitComponents(container.ParentPage, component);
            return component;
        }

        public static IComponent CreateComponent<T>(IPage page, params object[] additionalArgs)
        {
            var component = CreateComponent(page, page, typeof(T), new WebComponentAttribute(additionalArgs));
            InitComponents(page, component);
            return component;
        }

        /// <summary>
        ///     ������� ��������� � ���������������� ��� ��������� ����������
        ///     (���������� ��������� WebComponent)
        /// </summary>
        public static IComponent CreateComponent<T>(
            IPage page,
            object componentContainer,
            params object[] additionalArgs)
        {
            var component = CreateComponent(
                page,
                componentContainer,
                typeof(T),
                new WebComponentAttribute(additionalArgs));
            InitComponents(page, component);
            return component;
        }

        public static IComponent CreateComponent(IPage page, Type type, params object[] additionalArgs)
        {
            var component = CreateComponent(page, page, type, new WebComponentAttribute(additionalArgs));
            InitComponents(page, component);
            return component;
        }

        public static IComponent CreateComponent(
            IPage page,
            object componentContainer,
            Type type,
            IComponentAttribute attribute)
        {
            var args = typeof(ItemBase).IsAssignableFrom(type)
                           ? new List<object> { componentContainer } // �������
                           : new List<object> { page };
            if (attribute.Args != null)
            {
                var container = componentContainer as IContainer;
                if (container != null)
                {
                    // ������������� ������������� ���� � ����������
                    for (var i = 0; i < attribute.Args.Length; i++)
                    {
                        attribute.Args[i] = CreateInnerSelector(container, attribute.Args[i]);
                    }
                }
                args.AddRange(attribute.Args);
            }
            var component = (IComponent)Activator.CreateInstance(type, args.ToArray());
            component.ComponentName = attribute.ComponentName;
            return component;
        }

        private static object CreateInnerSelector(IContainer container, object argument)
        {
            var argumentString = argument as string;
            if (argumentString != null && argumentString.StartsWith("root:"))
            {
                return container.InnerScss(argumentString.Replace("root:", string.Empty));
            }
            return argument;
        }

        /// <summary>
        ///     ���������������� ����������
        /// </summary>
        /// <remarks>
        ///     ����� Reflection ����� � ���������������� ��� ���� ������� ����������� ��������� IComponent
        /// </remarks>
        public static void InitComponents(IPage page, object componentsContainer)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page", "page cannot be null");
            }
            if (componentsContainer == null)
            {
                componentsContainer = page;
            }
            var type = componentsContainer.GetType();
            var components = GetComponents(type);
            foreach (var memberInfo in components.Keys)
            {
                var attribute = components[memberInfo];
                IComponent instance;
                if (memberInfo is FieldInfo)
                {
                    var fieldInfo = memberInfo as FieldInfo;
                    instance = CreateComponent(page, componentsContainer, fieldInfo.FieldType, attribute);
                    fieldInfo.SetValue(componentsContainer, instance);
                }
                else if (memberInfo is PropertyInfo)
                {
                    var propertyInfo = memberInfo as PropertyInfo;
                    instance = CreateComponent(page, componentsContainer, propertyInfo.PropertyType, attribute);
                    propertyInfo.SetValue(componentsContainer, instance);
                }
                else
                {
                    throw new NotSupportedException("Unknown member type");
                }
                page.RegisterComponent(instance);
                InitComponents(page, instance);
            }
        }

        /// <summary>
        ///     �������� ������ �����-����������� ����(������� ����-���������� ������������ �����)
        /// </summary>
        private static Dictionary<MemberInfo, IComponentAttribute> GetComponents(Type type)
        {
            var components = new Dictionary<MemberInfo, IComponentAttribute>();
            // �������� ������ �����
            var members =
                type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Cast<MemberInfo>()
                    .ToList();
            // �������� ������ �������
            members.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            var attributeType = typeof(IComponentAttribute);
            foreach (var field in members)
            {
                var attributes = field.GetCustomAttributes(attributeType, true);
                if (attributes.Length == 0)
                {
                    continue;
                }
                components.Add(field, attributes[0] as IComponentAttribute);
            }
            return components;
        }

        /// <summary>
        ///     �������� �� ����/�������� ������ �����������
        /// </summary>
        private static bool IsComponent(FieldInfo fieldInfo)
        {
            var type = typeof(IComponent);
            return type.IsAssignableFrom(fieldInfo.FieldType);
        }
    }

    [TestFixture]
    public class WebPageBuilderTest
    {
        private class Container : ContainerBase
        {
            [WebComponent("root:div[text()='mytext']")]
            public Component Component1;

            [WebComponent("//div[text()='mytext']")]
            public Component Component2;

            public Container(IPage parent, string rootScss)
                : base(parent, rootScss)
            {
            }
        }

        private class Component : ComponentBase
        {
            public readonly string Xpath;

            public Component(IPage page, string xpath)
                : base(page)
            {
                this.Xpath = xpath;
            }

            public override bool IsVisible()
            {
                throw new NotImplementedException();
            }
        }

        private class Page : PageBase
        {
        }

        [Test]
        public void DoNotAddRootWithouPrefix()
        {
            var page = new Page();
            var container = new Container(page, "//*[@id='rootelementid']");
            WebPageBuilder.InitComponents(page, container);
            Assert.AreEqual(
                "//div[text()" + "='mytext']",
                container.Component2.Xpath,
                "������������� xpath �� �������������� � ����������");
        }

        [Test]
        public void ReplacePrefixWithRootSelector()
        {
            var page = new Page();
            var container = new Container(page, "//*[@id='rootelementid']");
            WebPageBuilder.InitComponents(page, container);
            Assert.AreEqual(
                "//*[@id='rootelementid']/descendant::div[text()='mytext']",
                container.Component1.Xpath,
                "������������� xpath �� �������������� � ����������");
        }
    }
}