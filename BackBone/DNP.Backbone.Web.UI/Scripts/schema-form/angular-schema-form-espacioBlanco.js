angular.module('schemaForm').config(['schemaFormProvider', 'schemaFormDecoratorsProvider', 'sfPathProvider',
    function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {

        var label = function (name, schema, options) {
            if ((schema.type === 'espacioBlanco') && schema.format == 'espacioBlanco') {
                var f = schemaFormProvider.stdFormObj(name, schema, options);
                f.key = options.path;
                f.type = 'espacioBlanco';
                options.lookup[sfPathProvider.stringify(options.path)] = f;
                return f;
            }
        };

        schemaFormProvider.defaults.string.unshift(label);

        schemaFormDecoratorsProvider.addMapping(
            'bootstrapDecorator',
            'espacioBlanco',
            '/Scripts/schema-form/directives/espacioBlanco.html'
        );
    }
]).directive('espacioBlancoDirective', ['$filter', function ($filter) {
    return {
        restrict: 'A',
        scope: false,
        controller: ['$scope', espacioBlancoControllerFunction],
        link: function (scope, iElement, iAttrs, ngModelCtrl) {
            scope.ngModel = ngModelCtrl;
            scope.filter = $filter
        }
    };
    }]);



var espacioBlancoControllerFunction = function ($scope) {
    function constructor() { }
    constructor();
};
