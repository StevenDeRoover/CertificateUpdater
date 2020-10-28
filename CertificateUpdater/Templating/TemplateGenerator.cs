using HandlebarsDotNet;
using System;

namespace CertificateUpdater.Templating
{
    /// <summary>
    /// Factory to create a TemplateGenerator
    /// </summary>
    public static class TemplateGeneratorFactory
    {
        /// <summary>
        /// Creates a TemplateGenerator
        /// </summary>
        /// <typeparam name="TTemplateModel"></typeparam>
        /// <param name="template">The template to render</param>
        /// <param name="model">The model used to render the template</param>
        /// <returns>The complete rendered template</returns>
        public static TemplateGenerator<TTemplateModel> Create<TTemplateModel>(string template, TTemplateModel model)
            where TTemplateModel : class
        {
            return new TemplateGenerator<TTemplateModel>(template, model);
        }

        /// <summary>
        /// Creates a TemplateGenerator
        /// </summary>
        /// <typeparam name="TTemplateModel"></typeparam>
        /// <param name="masterTemplate">Mustache template, with a partial reference to <b>template</b></param>
        /// <param name="partialName">Has default 'template'</param>
        /// <param name="template">The partial template rendered in the masterTemplate</param>
        /// <param name="model">The model used to render the template</param>
        /// <returns>The complete rendered template</returns>
        public static TemplateGenerator<TTemplateModel> Create<TTemplateModel>(string masterTemplate, string template, TTemplateModel model)
            where TTemplateModel : class
        {
            return new MasterTemplateGenerator<TTemplateModel>(masterTemplate, template, model);
        }

        /// <summary>
        /// Creates a TemplateGenerator
        /// </summary>
        /// <typeparam name="TTemplateModel"></typeparam>
        /// <param name="masterTemplate">Mustache template, with a partial reference to <b>template</b></param>
        /// <param name="partialName">Mustache reference to the partial, f.i. if partialName = 'template', the mustache template can contain {{> template}}.  It will render the template</param>
        /// <param name="template">The partial template rendered in the masterTemplate</param>
        /// <param name="model">The model used to render the template</param>
        /// <returns>The complete rendered template</returns>
        public static TemplateGenerator<TTemplateModel> Create<TTemplateModel>(string masterTemplate, string partialName, string template, TTemplateModel model)
            where TTemplateModel : class
        {
            return new MasterTemplateGenerator<TTemplateModel>(masterTemplate, partialName, template, model);
        }
    }

    public class TemplateGenerator<TTemplateModel>
        where TTemplateModel : class
    {
        private IHandlebars _templateEngine;
        public string Template { get; }
        public TTemplateModel Model { get; }
        protected IHandlebars TemplateEngine { get => _templateEngine; set => _templateEngine = value; }

        internal TemplateGenerator(string template, TTemplateModel model)
        {
            TemplateEngine = Handlebars.Create();
            Template = template;
            Model = model;
        }

        protected virtual Func<TTemplateModel, string> CreateGenerator()
        {
            return TemplateEngine.Compile(Template);
        }
        public string Generate()
        {
            return CreateGenerator()(Model);
        }
    }

    public class MasterTemplateGenerator<TTemplateModel> : TemplateGenerator<TTemplateModel>
        where TTemplateModel : class
    {
        public string MasterTemplate { get; }

        private readonly string _partialName;

        internal MasterTemplateGenerator(string masterTemplate, string template, TTemplateModel model) : this(masterTemplate, "template", template, model)
        {
            MasterTemplate = masterTemplate;
        }

        internal MasterTemplateGenerator(string masterTemplate, string partialName, string template, TTemplateModel model) : base(template, model)
        {
            MasterTemplate = masterTemplate;
            _partialName = partialName;
        }

        protected override Func<TTemplateModel, string> CreateGenerator()
        {
            TemplateEngine.RegisterTemplate(_partialName, Template);
            return TemplateEngine.Compile(MasterTemplate);
        }
    }
}
