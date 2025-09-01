// Karma configuration
// Generated on Mon Dec 18 2017 17:17:56 GMT-0500 (SA Pacific Standard Time)

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
		'Scripts/angular.js',
		'Scripts/angular-animate.js',
		'Scripts/angular-mocks.js',
		'Scripts/angular-route.js',
        'Scripts/jquery-3.2.1.js',
        'Scripts/angular-touch.js',
        'Scripts/angular-block-ui.js',
		//'Scripts/jquery-3.2.1.slim.js',
		'Scripts/bootstrap.js',		
		'Scripts/jquery.validate.js',
		'Scripts/modernizr-2.8.3.js',
		'Scripts/ng-table.js',
		'Scripts/ngStorage.js',
		'Scripts/respond.js',
		'Scripts/select.js',
		'Scripts/ui-bootstrap-2.5.0.js',
        'Scripts/underscore.js',
        'Scripts/ui-grid.js',
        'Scripts/draggable-rows.js',

        //Comunes
        'src/app/test/variablesGlobales.js',

		//Aplicacion
        'src/app/app.js',

        'src/app/comunes/core/core.module.js',
		'src/app/comunes/rutasApp.js',
		'src/app/comunes/constantesApp.js',
        'src/app/comunes/serviciosApp.js',

        //Panel Principal
        'src/app/panelPrincial/servicioPanelPrincipal.js',
        'src/app/panelPrincial/controladorPanelPrincipal.js',

        //Test
        'src/app/test/mockTest.js',

        //Test Accion
        'src/app/acciones/accionesTest/ejecucionFormulariosControllerTest.js',
        'src/app/acciones/servicios/ejecutorReglasServicios.js',
        'src/app/acciones/consultarAcciones/ejecucionFormulariosController.js',


        //Tests panel principal
        'src/app/panelPrincial/test/plantillaPanelPrincipalTest.js',

        //schema-form
        "Scripts/schema-form/tv4.js",
        "Scripts/schema-form/ObjectPath.js",
        "Scripts/schema-form/schema-form.js",
        "Scripts/schema-form/bootstrap-decorator.min.js",
        "Scripts/schema-form/angular-schema-form-dynamic-select.js",
        "Scripts/angular-schema-form-datepicker/bootstrap-datepicker.min.js",
        "Scripts/schema-form/angular-schema-form-grid.js",
        "Scripts/schema-form/angular-schema-form-accordion.js",
        "Scripts/schema-form/angular-schema-form-title.js",
        "Scripts/Slick/angular-slick.js",
        "Scripts/datetimepicker.js",
        "Scripts/datetimepicker.templates.js",
        "Scripts/pickadate/lib/picker.js",
        "Scripts/pickadate/lib/picker.date.js",
        "Scripts/pickadate/translations/nl_NL.js",
        'Scripts/angular-date-time-input.js',

		//HTML
		'src/app/*.html'
    ],


    // list of files to exclude
    exclude: [
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
		'src/app/*.js': ['coverage'],
		
		//Html
		'src/app/*.html': ['ng-html2js']
    },

	ngHtml2JsPreprocessor: {
            stripPrefix: 'app/',
            prependPrefix: '/',
            moduleName: 'templates'
        },

    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress', 'coverage'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false,

    // Concurrency level
    // how many browser should be started simultaneous
    concurrency: Infinity
  })
}
