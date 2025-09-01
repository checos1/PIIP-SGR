angular.module('schemaForm').config(['schemaFormProvider', 'schemaFormDecoratorsProvider', 'sfPathProvider',
    function(schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {   	
    var accordion = function (name, schema, options) {
        if ((schema.type === 'accordion') && schema.format == 'accordion') {
            var f = schemaFormProvider.stdFormObj(name, schema, options);
            f.key = options.path;
            f.type = 'accordion';
            options.lookup[sfPathProvider.stringify(options.path)] = f;
           
            return f;
        }
    };

    schemaFormProvider.defaults.string.unshift(accordion);

    schemaFormDecoratorsProvider.addMapping(
        'bootstrapDecorator',
        'accordion',
        '/Scripts/schema-form/directives/accordion.html'
    );
    
    }]).directive('accordeondirective', ['$filter', function ($filter) {
        return {
            restrict: 'A',
            scope: false,
            controller: ['$scope', accordeonControllerFunction],
            link: function (scope, iElement, iAttrs, ngModelCtrl) {
                scope.ngModel = ngModelCtrl;
                scope.filter = $filter
            }
        };
    }]);

var accordeonControllerFunction = function ($scope) {
    console.log($scope.form.panels);

};