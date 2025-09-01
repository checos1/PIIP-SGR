(function () {
    'use strict';

    angular.module('backbone.core', [
        'ngRoute',
        'ngAnimate',
        'ui.bootstrap',
        'ngStorage',
        'schemaForm',
        'ui.grid',
        'ui.grid.edit',
        'ui.grid.rowEdit',
        'ui.grid.cellNav',
        'ui.grid.exporter',
        'ui.grid.resizeColumns',
        'ui.grid.moveColumns',
        'ui.grid.pagination',
        'ui.grid.saveState',
        'ui.grid.selection',
        'ui.bootstrap.datetimepicker',
        'ui.dateTimeInput',
        'blockUI',
        'ngFileSaver',
        //Comunes
        'backbone.core.filtros',
        'backbone.core.directives'
    ]);
})();