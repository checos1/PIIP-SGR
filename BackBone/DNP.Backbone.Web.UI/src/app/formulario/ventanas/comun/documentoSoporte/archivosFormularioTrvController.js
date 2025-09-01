(function () {
    'use strict';

    archivosFormularioTrvController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'archivoServicios',
        '$timeout',
        'justificacionCambiosServicio',
        'FileSaver',
        'transversalSgrServicio',
        '$uibModal',
        'documentoSoporteServicios'
    ];

    function archivosFormularioTrvController(
        $scope,
        $sessionStorage,
        utilidades,
        archivoServicios,
        $timeout,
        justificacionCambiosServicio,
        FileSaver,
        transversalSgrServicio,
        $uibModal,
        documentoSoporteServicios
    ) {
        var vm = this;
        vm.descargarArchivoBlob = descargarArchivoBlob;
        vm.eliminarArchivoBlob = eliminarArchivoBlob;
        vm.initArchivosFormulario = initArchivosFormulario;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.IdAccion = $sessionStorage.idAccion;
        vm.tipotramiteid = $sessionStorage.tipoTramiteId;
        vm.tramiteid = $sessionStorage.TramiteId;
        vm.puedeGenerarverReq = false;
        vm.proyectoId = $sessionStorage.proyectoId;
        //Controlar para verificar si se requiere versionamiento
        vm.permiteVersionamiento = true;

        //Para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        vm.modelo = {
            coleccion: "tramites",
            ext: ".pdf",
            codigoProceso: $sessionStorage.numeroTramite,
            descripcionTramite: $sessionStorage.descripcionTramite,
            idInstancia: $sessionStorage.idInstancia,
            idAccion: $sessionStorage.idAccion,
            section: vm.section,
            idTipoTramite: vm.tipotramiteid
        };
        vm.listaArchivos = [];
        vm.disabled = $sessionStorage.soloLectura;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        vm.listaTipoArchivosFiltro = [];
        vm.listaPrincipal = [];
        vm.listaAnidada = [];

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                $sessionStorage.nombrecomponentepaso = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            }
        });

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            vm.disabled = true;
            $("#btnExaminar").attr('disabled', true);
        });

        //Modal que carga documentos
        vm.abrirModal = function () {
            var modalCargarDoc = $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/comun/documentoSoporte/modalCargarDoc/cargarDocModal.html',
                controller: 'cargarDocModalController',
                size: 'lg'
            });

            // Ejecutar cuando el modal se cierre
            modalCargarDoc.result.then(function () {
                delete $sessionStorage.listaTipoArchivosFiltro;
                delete $sessionStorage.nombrecomponentepaso;
                inicializarListas();
                consultarArchivosTramite();
            }, function () {
                // Se ejecuta si el modal se cierra sin aceptar (por ejemplo, al presionar ESC o al hacer clic fuera)
                delete $sessionStorage.listaTipoArchivosFiltro;
                delete $sessionStorage.nombrecomponentepaso;
                inicializarListas();
                consultarArchivosTramite();
            });
        };

        //Modal que carga version de documentos        
        vm.abrirModalVersion = function (entityData) {

            $sessionStorage.entityData = entityData;

            var modalCargarDoc = $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/comun/documentoSoporte/modalVersionCargarDoc/cargarVersionDocModal.html',
                controller: 'cargarVersionDocModalController',
                controllerAs: 'vm',
                size: 'lg'
            });

            // Ejecutar cuando el modal se cierre
            modalCargarDoc.result.then(function () {
                delete $sessionStorage.entityData;
                delete $sessionStorage.nombrecomponentepaso;
                inicializarListas();
                consultarArchivosTramite();
            }, function () {
                // Se ejecuta si el modal se cierra sin aceptar (por ejemplo, al presionar ESC o al hacer clic fuera)
                delete $sessionStorage.entityData;
                delete $sessionStorage.nombrecomponentepaso;
                inicializarListas();
                consultarArchivosTramite();
            });
        };

        function initArchivosFormulario() {
            $sessionStorage.sessionDocumentos = 0;
            consultarArchivosTramite();
        }

        function inicializarListas() {
            vm.listaPrincipal = [];
            vm.listaAnidada = [];
        }

        function consultarArchivosTramite() {
            transversalSgrServicio.SGR_Transversal_LeerParametro("IdFlujoDocSoportexInstancia")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data) {
                        var flujos = respuestaParametro.data.Valor;
                        var esInstancia = flujos.toUpperCase().includes($sessionStorage.flujoSeleccionado.toUpperCase());
                        consultarArchivos(esInstancia);
                    }
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al obtener los parametros.');
                    return;
                });
        }

        function consultarArchivos(esInstancia) {
            let param;

            //Consultar por instancia
            if (esInstancia) {
                if (vm.nivel === vm.nivelarchivo)
                    param = {
                        idInstancia: vm.modelo.idInstancia,
                        //proyectoId: vm.proyectoId.toString(),
                        idNivel: vm.nivel
                    };
                else {
                    param = {
                        idInstancia: vm.modelo.idInstancia,
                        //proyectoId: vm.proyectoId.toString(),
                        //idRol: vm.rol
                        section: vm.modelo.section,
                        idAccion: vm.modelo.idAccion,
                        idNivel: vm.nivel
                    };
                }
                //Consultar por proyecto
            } else {
                if (vm.nivel === vm.nivelarchivo)
                    param = {
                        proyectoId: vm.proyectoId.toString(),
                        idNivel: vm.nivel
                    };
                else {
                    param = {
                        //idInstancia: vm.modelo.idInstancia, 
                        proyectoId: vm.proyectoId.toString(),
                        //idRol: vm.rol
                        section: vm.modelo.section,
                        idAccion: vm.modelo.idAccion,
                        idNivel: vm.nivel
                    };
                }
            }

            if (vm.rol === '')
                param = {
                    idInstancia: vm.modelo.idInstancia
                };

            if (vm.section != null && vm.section != undefined && vm.section != '') {
                param.section = vm.section;
            }

            vm.listaArchivos = [];
            $sessionStorage.sessionDocumentos = 0;
            vm.listaTipoArchivosFiltro = [];

            documentoSoporteServicios.ObtenerListadoArchivosPIIP(param, vm.modelo.coleccion).then(function (response) {
                vm.listaArchivos = [];

                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                } else {
                    response.forEach(archivo => {
                        if (archivo.status !== 'Eliminado') {
                            if (archivo.nombre.length > 60) {
                                var descripcionTmp = archivo.nombre.split(" ");
                                archivo.nombre = '';
                                descripcionTmp.map(function (item) {
                                    var ctaTmp = 0;
                                    if (item.length > 60) {
                                        ctaTmp = Math.floor(item.length / 60);
                                        for (var i = 0; i < ctaTmp; i++) {
                                            archivo.nombre += item.substring(60 * i, 60) + " ";
                                        }
                                    }
                                    if ((item.length % 50) > 0) {
                                        archivo.nombre += item.substring(60 * ctaTmp, (60 * ctaTmp) + 60) + " ";
                                    }
                                })
                                descripcionTmp = archivo.metadatos.descripcion.split(" ");
                                archivo.metadatos.descripcion = '';
                                descripcionTmp.map(function (item) {
                                    var ctaTmp = 0;
                                    if (item.length > 60) {
                                        ctaTmp = Math.floor(item.length / 60);
                                        for (var i = 0; i < ctaTmp; i++) {
                                            archivo.metadatos.descripcion += item.substring(60 * i, 60) + " ";
                                        }
                                    }
                                    if ((item.length % 50) > 0) {
                                        archivo.metadatos.descripcion += item.substring(60 * ctaTmp, (60 * ctaTmp) + 60) + " ";
                                    }
                                })
                            };

                            // Guardar los tipos de documento que ya están cargados
                            var tipoDocumento = archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid;

                            vm.listaTipoArchivosFiltro.push({
                                tipoDocumentoId: tipoDocumento,
                                versionDocumentoSoporte: archivo.metadatos.versiondocumentosoporte === undefined ? '1' : archivo.metadatos.versiondocumentosoporte
                            });

                            // Filtrar para dejar solo el último con la versión mayor por tipo de documento
                            vm.listaTipoArchivosFiltro = Object.values(
                                vm.listaTipoArchivosFiltro.reduce((acc, curr) => {
                                    let version = parseInt(curr.versionDocumentoSoporte, 10);

                                    if (!acc[curr.tipoDocumentoId] || version > parseInt(acc[curr.tipoDocumentoId].versionDocumentoSoporte, 10)) {
                                        acc[curr.tipoDocumentoId] = curr;
                                    }

                                    return acc;
                                }, {})
                            );

                            //Transformar fechas
                            let fechaArchivo = new Date(archivo.fecha);
                            let fechaPaso = buscarFechaModificacionPaso();
                            let noPermiteVersion = fechaPaso ? fechaArchivo > fechaPaso : false;
                            let noPermiteEliminar = fechaPaso ? fechaArchivo < fechaPaso : false;

                            vm.listaArchivos.push({
                                codigoProceso: archivo.metadatos.codigoproceso,
                                descripcion: archivo.metadatos.descripcion,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? '' : archivo.metadatos.tipodocumento,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                datosDocumento: archivo.metadatos.datosdocumento,
                                nombreDocumento: archivo.nombre,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id,
                                nombre: archivo.nombre,
                                versionDocumentoSoporte: archivo.metadatos.versiondocumentosoporte === undefined ? '1' : archivo.metadatos.versiondocumentosoporte,
                                noPermiteVersion: noPermiteVersion,
                                noPermiteEliminar: noPermiteEliminar,
                                //Temporal Pruebas
                                fechaPaso: fechaPaso,
                                fechaArchivo: fechaArchivo
                            });

                        }
                    });

                    if (response.filter(p => p.status !== 'Eliminado').length === 0 && vm.guardar === 1) {
                        eliminarCapitulosModificados();/// Aqui borrar
                    }

                    $sessionStorage.listaTipoArchivosFiltro = vm.listaTipoArchivosFiltro;
                    vm.organizarDocumentos(vm.listaArchivos);
                    //Eliminar despues de pruebas
                    console.log("listaArchivos");
                    console.log(vm.listaArchivos);
                    console.log("listaPrincipal");
                    console.log(vm.listaPrincipal);
                    console.log("listaAnidada");
                    console.log(vm.listaAnidada);
                }
            });
        };

        // Busca coincidencia en listadoAccionesTramite y devuelve FechaModificacion como objeto Date
        function buscarFechaModificacionPaso() {
            var nombreAccion = $sessionStorage.InstanciaSeleccionada.NombreAccion;
            var listadoAcciones = $sessionStorage.listadoAccionesTramite;

            for (var i = 0; i < listadoAcciones.length; i++) {
                if (listadoAcciones[i].Nombre === nombreAccion)
                    return listadoAcciones[i].FechaModificacion ? new Date(listadoAcciones[i].FechaModificacion) : null;
            }

            // Devuelve null si no se encuentra coincidencia
            return null;
        }

        vm.organizarDocumentos = function (documentos) {
          
            const documentosPorTipo = documentos.reduce((acc, doc) => {
              
                if (!acc[doc.tipoDocumentoId]) {
                    acc[doc.tipoDocumentoId] = [];
                }
                acc[doc.tipoDocumentoId].push(doc);
                return acc;
            }, {});
                       
            Object.keys(documentosPorTipo).forEach(tipoDocumentoId => {
                const docsDeTipo = documentosPorTipo[tipoDocumentoId];

                docsDeTipo.sort((a, b) => {
                    return b.versionDocumentoSoporte - a.versionDocumentoSoporte;
                });
               
                const docMasReciente = docsDeTipo[0];
                vm.listaPrincipal.push(docMasReciente);
             
                if (docsDeTipo.length > 1) {
                    vm.listaAnidada[tipoDocumentoId] = docsDeTipo.slice(1);
                }
            });
       
            vm.listaAnidada = Object.fromEntries(
                Object.entries(vm.listaAnidada).filter(([key, value]) => value && value.length > 0)
            );
        };

        vm.toggleVersions = function (objArchivo) {
            objArchivo.showVersions = !objArchivo.showVersions;
        };

        function eliminarArchivoBlob(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {

                    archivoServicios.cambiarEstadoDataArchivo(entity.idMongo, vm.modelo.coleccion).then(function () {
                        guardarCapituloModificado(0);
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        inicializarListas();
                        consultarArchivosTramite();
                    })
                        .then(function () {
                            $timeout(function () {
                                swal({
                                    title: "<span style='color:#069169'>El documento ha sido eliminado con éxito.<span>",
                                    text: "<div style='padding-bottom:2rem !important;'></div><button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>",
                                    type: "success",
                                    showCancelButton: false,
                                    confirmButtonText: "Aceptar",
                                    cancelButtonText: "Cancelar",
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    customClass: 'sweet-alertTittleSucces',
                                    html: true
                                });
                            }, 50);
                        });
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El documento será eliminado.");
        }

        function descargarArchivoBlob(entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, vm.modelo.coleccion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                FileSaver.saveAs(blob, entity.nombre);
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }

        //Para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado(paramModificado) {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: paramModificado,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia
            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //Para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombrecomponentepaso);
            vm.seccionCapitulo = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campomensajeerror2 = document.getElementById(vm.nombreComponente + "A" + "-archivo-error");
            if (campomensajeerror2 != undefined) {
                campomensajeerror2.innerHTML = "";
                campomensajeerror2.classList.add('hidden');
            }
            var campomensajeerror3 = document.getElementById(vm.nombreComponente + "B" + "-archivo-error");
            if (campomensajeerror3 != undefined) {
                campomensajeerror3.innerHTML = "";
                campomensajeerror3.classList.add('hidden');
            }
        }

        vm.notificacionValidacionPadre = function (errores) {

            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarErrores2 = function (errores) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "A-archivo-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarErrores3 = function (errores) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "B-archivo-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionArchivo = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'VFO006': vm.validarValoresVigenciaInformacionArchivo,
            'VDSDV': vm.validarValoresVigenciaInformacionArchivo,
            'SGRVDP1': vm.validarErrores2,
            'SGRVDP2': vm.validarErrores3,
            'SGRAVAL2': vm.validarValoresVigenciaInformacionArchivo
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/

    }

    angular.module('backbone').component('archivosFormularioTrv', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/comun/documentoSoporte/archivosFormularioTrv.html",
        controller: archivosFormularioTrvController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nivelarchivo: '@',
            tipoarchivo: '@',
            objetonegocioid: '@',
            nombrecomponentepaso: '@',
            idtipotramitepresupuestal: '@',
        }
    });
})();