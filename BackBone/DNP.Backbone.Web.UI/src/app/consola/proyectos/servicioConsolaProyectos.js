(function () {
    'use strict';

    angular.module('backbone').factory('servicioConsolaProyectos', servicioConsolaProyectos);
    servicioConsolaProyectos.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    // descripción de la columnas
    var columnasPorDefectoProyecto = ['Sector', 'Entidad', 'ID', 'BPIN', 'Estado','Nombre del proyecto',  'Horizonte'];
    var columnasDisponiblesProyecto = [];

    function servicioConsolaProyectos($q, $http, $location, constantesBackbone) {

        return {
            columnasDisponiblesProyecto: columnasDisponiblesProyecto,
            columnasPorDefectoProyecto: columnasPorDefectoProyecto,
            obtenerInbox: obtenerInbox,
            obtenerSectores: obtenerSectores,
            obtenerEntidades: obtenerEntidades,
            obtenerEstadoProyectos: obtenerEstadoProyectos,
            obtenerHistorialProyecto: obtenerHistorialProyecto,
            obtenerInstanciasProyecto: obtenerInstanciasProyecto,
            InsertarAuditoriaProyecto: InsertarAuditoriaProyecto,
            imprimirPDFConsolaProyectos: imprimirPDFConsolaProyectos,
            obtenerExcel: obtenerExcel,
            obtenerDocumentosAdjuntos: obtenerDocumentosAdjuntos,
            obtenerDocumentoAdjunto: obtenerDocumentoAdjunto,
            obtenerVigenciasProyectos: obtenerVigenciasProyectos,
            obtenerSoportesProyectos: obtenerSoportesProyectos,
            descargarArchivos: descargarArchivos,
            ObtenerEntidadesPorSector: ObtenerEntidadesPorSector,
            ObtenerMarcas: ObtenerMarcas,
            obtenerIdAplicacion
        }
        
        function ObtenerMarcas() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMarcas);
        }
        function ObtenerEntidadesPorSector(sectorId, tipoEntidad) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesPorSector + "?sectorId=" + sectorId + "&tipoEntidad=" + tipoEntidad + "&usuarioDNP=" + usuarioDNP);
        }
        function descargarArchivos(archivos) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDescargarArchivosProyecto;
            return $http.post(url, archivos);
        }
        function obtenerSoportesProyectos(documentosFiltro) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSoportesProyecto, documentosFiltro);
        }

        function obtenerInbox(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectos, inboxDto);
        }


        function obtenerSectores(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectosListaSectores, peticion);
        }

        function obtenerEntidades(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectosListaEntidades, peticion);
        }

        function obtenerEstadoProyectos(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectosListaEstadosProyecto, peticion);
        }

        function obtenerVigenciasProyectos(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectosListaVigenciasProyectos, peticion);
        }

        function obtenerDocumentosAdjuntos(idProyecto) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDocumentosAdjuntos + '?idProyecto=' + idProyecto);
        }

        function obtenerDocumentoAdjunto(idProyecto, nombreArchivo) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDocumentoAdjunto;
            return $http.post(url, { idProyecto, nombreArchivo }, { responseType: 'blob' });
        }

        /**
         * 
         * @description Insertar el historial de cambio de entidad del proyectoa ctual
         * @param {object} auditoriaProyecto. Información del cambio de entidades: { 
         *                                                                              EntidadOrigenId: Number, 
         *                                                                              EntidadOrigen: String, 
         *                                                                              EntidadDestinoId:Number, 
         *                                                                              EntidadDestino: String, 
         *                                                                              ProyectoId: Number,
         *                                                                              SectorId: Number
         *                                                                          }
         * @param {object} usuario. Datos de la cuenta de usuario actual: { IdPIIP: UUID, NombreCuenta: String }
         */
        function InsertarAuditoriaProyecto(auditoriaProyecto, usuario) {
            try {
                return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneInsertarAuditoriaProyecto, { auditoria: auditoriaProyecto, usuario: usuario });
            }
            catch (exception) {
                throw { message: `servicioConsolaProyecto.InsertarAuditoriaProyecto: ${exception.message}` }
            }
        }

        /**
         * 
         * @description Obtiene el historial de intercambio entre entidades del proyecto actual
         * @param {Number} proyectoId . Identificador del proyecto actual
         */
        function obtenerHistorialProyecto(proyectoId, usuario) {
            try {
                return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAuditoriaProyecto, {
                    params: {
                        proyectoId: proyectoId, usuarioDNP: usuario
                    }
                });
            }
            catch (exception) {
                throw { message: `servicioConsolaProyectos.obtenerProyectoEntidades: ${exception.message}` }
            }
        }

        /** 
         * @descirption . Obtiene la petición http response del archivo Excel generado con la información proporcionada 
         * @param {Object} datos. Es una instancia de la clase Dominio.Dto.Proyecto.ProyectoDto 
         */
        function obtenerExcel(datos) {
            try {

                var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneExcelConsolaProyectos;
                return $http.post(url, datos);
            }
            catch (exception) {
                throw { message: `servicioConsolaProyectos.obtenerExcel => ${exception.message}` }
                throw { message: `servicioConsolaProyectos.obtenerHistorialProyecto: ${exception.message}` }
            }
        }

        /**
         * @description . Genera un archivo PDF a partir de la información proporcionada
         * @param {object} datos . Arreglo de objetos para imprimir en el archivo PDF
         */
        function imprimirPDFConsolaProyectos(datos) {
            try {

                let url = `${urlPDFBackbone}${constantesBackbone.apiBackboneColsaProyectosPDF}`;

                return $http.post(url, { datos: datos });
            }
            catch (exception) {
                throw { message: `servicioConsolaProyectos.imprimirPDFConsolaProyectos : ${exception.message}` };
            }
        }

        /**
         * NOTE: Es un metodo para obtener datos FICTICIOS(FAKE)
         * @author Eduardo Antonio Villamil Pérez
         * @description Obtiene un objeto JSON desde el archivo con el nombre especificado del JSON
         * @param {String} nombreJsoon . Nombre del archivo json 
         */
        function obtenerJSONLocal(nombreJson) {
            try {

                return $http({
                    method: 'GET',
                    'Content-Type': 'application/json;charset=utf-8',
                    url: `http://localhost:3024/src/assets/${String(nombreJson)}.json`
                });
            }
            catch (exception) {
                throw { message: `servicioCargarDatos.obtenerJSONLocal => ${exception.message}` }
            }
        }

        function obtenerInstanciasProyecto(peticionObtenerInbox, proyectoFiltro) {
            const dto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaProyectosInstancias, dto);
        }

        function obtenerIdAplicacion(idObjetoNegocio) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerIdAplicacionPorBpin + "?idObjetoNegocio=" + idObjetoNegocio);
        }

    }
})();