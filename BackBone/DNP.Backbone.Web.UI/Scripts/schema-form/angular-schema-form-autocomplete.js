angular.module('schemaForm').config(['schemaFormProvider', 'schemaFormDecoratorsProvider', 'sfPathProvider',
    function(schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {

    	
    var autocomplete = function (name, schema, options) {
        if ((schema.type === 'autocomplete') && schema.format == 'autocomplete') {
            var f = schemaFormProvider.stdFormObj(name, schema, options);
            f.key = options.path;
            f.type = 'autocomplete';
            options.lookup[sfPathProvider.stringify(options.path)] = f;
           
            return f;
        }
    };

    schemaFormProvider.defaults.string.unshift(autocomplete);

    schemaFormDecoratorsProvider.addMapping(
        'bootstrapDecorator',
        'autocomplete',
        '/Scripts/shema-form/directives/datepicker.html'
    );

    schemaFormDecoratorsProvider.createDirective(
        'autocomplete',
        '/Scripts/shema-form/directives/autocomplete.html'
    );   
}]);