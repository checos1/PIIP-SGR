(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = constantesFormularios.controlListaAutocompletable;

            schemaFormProvider.defaults.string.unshift(function (name, schema, options) {
                if (schema.type === control) {
                    var f = schemaFormProvider.stdFormObj(name, schema, options);
                    f.key = options.path;
                    f.type = schema.type;
                    options.lookup[sfPathProvider.stringify(options.path)] = f;
                    return f;
                }
            });

            schemaFormDecoratorsProvider.addMapping(
                "bootstrapDecorator",
                control,
                "/Scripts/schema-form/directives/" + control + ".html"
            );
        }
    ]);

    angular.module("backbone").controller("listaAutocompletableController", ["$scope",
        function ($scope) {
        }
    ]);
})();