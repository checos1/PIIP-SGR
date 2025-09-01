try {
    (function() {
        'use strict';
        let module = angular.module('backbone.entidades');

        /**
         * servicioEntidadProyectos . Servicio de Angular js para el modulo de proyectos en la sección Entidades
         * @param {object} $http. Directiva HTTP de angular js para peticiones http request 
         * @param {Function} constantesBackbone. Es un conjunto de constantes para toda la aplicaciòn Backbone
         * @param {Function} configurarEntidadRolSectorServicio. Servicio que contiene las funciones yc configuraciones de los roles de usuario y sectores por servicio
         * @param {Function} autorizacionServicios . Servicio que contien las fucniones y operaciones, peticiones HTTP para obtener los permisos de usuario
         * @param {Function} constantesAutorizacion 
         */
        let servicioEntidadProyectos = function ($http, constantesBackbone, configurarEntidadRolSectorServicio, autorizacionServicios, constantesAutorizacion){

            let funciones = {};

            funciones ={

                columnasDefecto : ['Proyecto/BPIN', 'Estado', 'Flujo', 'Fecha'],

                /** 
                 * @description Obtiene la lista de configuraciones del rol del usuario actual
                 * @returns {Array}. Devuelve un arreglo de objetos con las diferentes configuraciones y accesos de la cuenta de usuario actual
                */
                obtenerAccesoRol : function(){
                    try {

                        return configurarEntidadRolSectorServicio.obtenerConfiguracionesRolSector({
                            usuarioDnp: usuarioDNP,
                            nombreAplicacion: nombreAplicacionBackbone
                        });
                    }
                    catch(exception){
                        throw { message: `servicioEntidadProyecto.obtenerAccesoRol: ${exception.message}` }
                    }
                },

                /**
                 * @description . Obtiene las entidades a las que tiene acceso el usuario actual
                 * @returns {Array} . Devuelve una arreglo de objetos con el identificador, nombre de las entidades y a qué tipo de entidad pertenecen
                 */
                obtenerEntidades: function(){
                    try {
                        return autorizacionServicios.obtenerEntidadesPorRoles();
                    }
                    catch(exception){
                        throw { message: `servicioEntidadProyecto.obtenerAccesoRol: ${exception.message}` }
                    }
                },

                /**
                 * @description Obtiene el catálogo estático de entidades
                 * @returns {Array}. Retorna un arreglo de objetos con las entidades {clave: String, nombre: String}
                 */
                obtenerTiposEntidad: function () {
                    try {
                        return autorizacionServicios.obtenerTiposEntidad();
                    }
                    catch(exception){
                        throw { message: `servicioEntidadProyectos.obtenerTiposEntidad: ${exception.message}`};
                    }
                },

                /**
                 * @description . Obtiene el catálogo de estados de un proyecto.
                 * @returns. {Array}. Retorna un arreglo de objetos con los diferentes estados que pueden haber para un proyecto
                 */
               obtenerEstadosProyecto: function(){
                   try {

                       return funciones.obtenerJSONLocal('estadosProyecto_test');
                   }
                   catch(exception){
                       throw {message: `servicioEntidadProyectos.obtenerEstadosProyecto: ${exception.message}`};
                   }
               },

                /**
                 * 
                 * @description . Obtiene la lista de proyectos de la entidad actual agrupado por sectores 
                 * @param {String} claveEntidad . Clave de identificación de la entidad actual
                 * @returns {Array} Retorna un arreglo de objetos con los proyectos de la entidad actual
                 */
                obtenerProyectoEntidad: function(claveEntidad){
                    try {
                        
                        return funciones.obtenerJSONLocal('proyectosEntidad_test');
                    }
                    catch(exception){
                        throw { exception: `servicioEntidadProyectos.obtenerProyectoEntidades: ${exception.message}` }
                    }
                },

                /**
                 * NOTE: Es un metodo para obtener datos FICTICIOS(FAKE)
                 * @author Eduardo Antonio Villamil Pérez
                 * @description Obtiene un objeto JSON desde el archivo con el nombre especificado del JSON
                 * @param {String} nombreJsoon . Nombre del archivo json 
                 */
                obtenerJSONLocal: function (nombreJson) {
                    try {

                        return $http({
                            method: 'GET',
                            'Content-Type': 'application/json;charset=utf-8',
                            url: `https://as-backbone-sitio-ntdev.azurewebsites.net/src/assets/${String(nombreJson)}.json`
                        });
                    }
                    catch (exception) {
                        throw { message: `servicioCargarDatos.obtenerJSONLocal => ${exception.message}` }
                    }
                }
            };

            return funciones;
        }

        servicioEntidadProyectos.$inject = ['$http', 'constantesBackbone', 'configurarEntidadRolSectorServicio', 'autorizacionServicios', 'constantesAutorizacion'];

        module.factory('servicioEntidadProyectos', servicioEntidadProyectos);
    })();
}
catch(exception){
    console.log('servicioEntidadProyectos => ', exception);
    alert('Ocurrió un error al cargar el servicio.');
}