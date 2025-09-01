angular.module('schemaForm').config(['schemaFormProvider', 'schemaFormDecoratorsProvider', 'sfPathProvider',
    function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {


        var title = function (name, schema, options) {
            if ((schema.type === 'title') && schema.format == 'title') {
                var f = schemaFormProvider.stdFormObj(name, schema, options);
                f.key = options.path;
                f.type = 'title';
                options.lookup[sfPathProvider.stringify(options.path)] = f;
                return f;
            }
        };
        
        schemaFormProvider.defaults.string.unshift(title);

        schemaFormDecoratorsProvider.addMapping(
            'bootstrapDecorator',
            'title',
            '/Scripts/schema-form/directives/title.html'
        );  
    }
]).directive('titledirective', ['$filter', function ($filter) {
    return {
        restrict: 'A',
        scope: false,
        controller: ['$scope', titleControllerFunction],
        link: function (scope, iElement, iAttrs, ngModelCtrl) {
            scope.ngModel = ngModelCtrl;
            scope.filter = $filter
        }
    };
}]);


var titleControllerFunction = function ($scope) {    
    function constructor() {
        $scope.model[$scope.form.key.slice(-1)[0]] = $scope.form.titulo;
    }

    constructor();
};


