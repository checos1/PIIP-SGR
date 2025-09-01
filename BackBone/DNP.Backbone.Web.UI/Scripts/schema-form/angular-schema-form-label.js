angular.module('schemaForm').config(['schemaFormProvider', 'schemaFormDecoratorsProvider', 'sfPathProvider',
    function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {

        var label = function (name, schema, options) {
            if ((schema.type === 'controlLabel') && schema.format == 'controlLabel') {
                var f = schemaFormProvider.stdFormObj(name, schema, options);
                f.key = options.path;
                f.type = 'controlLabel';
                options.lookup[sfPathProvider.stringify(options.path)] = f;
                return f;
            }
        };

        schemaFormProvider.defaults.string.unshift(label);

        schemaFormDecoratorsProvider.addMapping(
            'bootstrapDecorator',
            'controlLabel',
            '/Scripts/schema-form/directives/label.html'
        );
    }
]).directive('labelDirective', ['$filter', function ($filter) {
    return {
        restrict: 'A',
        scope: false,
        controller: ['$scope', labelControllerFunction],
        link: function (scope, iElement, iAttrs, ngModelCtrl) {
            scope.ngModel = ngModelCtrl;
            scope.filter = $filter
        }
    };
}]);


var labelControllerFunction = function ($scope) {
    function constructor() {}
    constructor();
};


