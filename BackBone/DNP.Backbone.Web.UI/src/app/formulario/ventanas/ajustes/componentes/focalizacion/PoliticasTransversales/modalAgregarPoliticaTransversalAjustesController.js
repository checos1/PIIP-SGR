(function () {
    'use strict';

    modalAgregarPoliticaTransversalAjustesController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'focalizacionAjustesServicio',
        'utilidades'
    ];

    function modalAgregarPoliticaTransversalAjustesController(
        $uibModalInstance,
        $sessionStorage,
        focalizacionAjustesServicio,
        utilidades
    ) {
        var vm = this;
        vm.init = init;
        vm.cerrar = $sessionStorage.close;
        vm.guardar = guardar;
        vm.listaPoliticasProyecto = [];
        
        vm.RespuestaAgregar = null;
        vm.guardo = false;

        vm.options = [];

        //vm.arreglolistaEntidades = [];

        vm.BPIN = $uibModalInstance.idObjetoNegocio;

        var lstRolesTodo = $uibModalInstance.usuario.roles;
        var lsRoles = [];
        for (var ls = 0; ls < lstRolesTodo.length; ls++)
            lsRoles.push(lstRolesTodo[ls].IdRol)

        var parametros = {
            "Aplicacion": nombreAplicacionBackbone,
            "ListaIdsRoles": lsRoles,
            "IdUsuario": usuarioDNP,
            "IdObjeto": $uibModalInstance.idInstancia,      //'88ea329d-f240-4868-9df7-86c74fb2ecfa',
            "InstanciaId": $uibModalInstance.idInstancia,   //'88ea329d-f240-4868-9df7-86c74fb2ecfa', 
            "IdFiltro": $uibModalInstance.idAccionAnterior
        }

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.obtenerPoliticasTransversales(vm.BPIN);
        }

        vm.obtenerPoliticasTransversales = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $uibModalInstance.usuario.permisos.IdUsuarioDNP;

            return focalizacionAjustesServicio.obtenerPoliticasTransversalesProyecto(bpin, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        //var cadena = '{"ProyectoId":null,"BPIN":"202200000000064","Politicas":[{"PoliticaId":1,"Politica":"ACTIVIDADES DE CIENCIA, TECNOLOGÍA E INNOVACIÓN","EnProyecto":true,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":2,"Politica":"CAMBIO CLIMÁTICO","EnProyecto":true,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":3,"Politica":"CAMPESINOS","EnProyecto":false,"EnSeguimiento":true,"EnFirme":false},{"PoliticaId":4,"Politica":"CONSTRUCCIÓN DE PAZ","EnProyecto":false,"EnSeguimiento":true,"EnFirme":false},{"PoliticaId":5,"Politica":"DESPLAZADOS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":6,"Politica":"DISCAPACIDAD E INCLUSIÓN SOCIAL","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":7,"Politica":"EQUIDAD DE LA MUJER","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":8,"Politica":"GESTIÓN DE RIESGO DE DESASTRES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":9,"Politica":"GRUPOS ÉTNICOS - COMUNIDADES RROM","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":10,"Politica":"GRUPOS ÉTNICOS - INDIGENAS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":11,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":12,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN PALENQUERA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":13,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN RAIZAL","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":14,"Politica":"PARTICIPACIÓN CIUDADANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":15,"Politica":"PLAN NACIONAL DE CONSOLIDACIÓN","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":16,"Politica":"RED UNIDOS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":17,"Politica":"SEGURIDAD ALIMENTARIA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":18,"Politica":"SEGURIDAD Y CONVIVENCIA CIUDADANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":19,"Politica":"TECNOLOGÍAS DE INFORMACIÓN Y COMUNICACIONES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":20,"Politica":"VÍCTIMAS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1506,"Politica":"POLITICA DE PRUEBA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1507,"Politica":"CONTROL A LA DEFORESTACIÓN Y GESTIÓN DE LOS BOSQUES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1508,"Politica":"PRIMERA INFANCIA, INFANCIA Y ADOLESCENCIA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1509,"Politica":"ZONAS FUTURO","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false}]}';
                        //var arregloGeneral = jQuery.parseJSON(cadena)

                        var arreglolistas = jQuery.parseJSON(respuesta.data);                        
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosGenerales = arregloGeneral.Politicas;

                        var listaPoliticasProy = [];

                        var enProyecto = false;

                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            if (arregloDatosGenerales[pl].EnProyecto) {
                                enProyecto = true;
                            }

                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                deshabilitar: enProyecto
                            }

                            listaPoliticasProy.push(politicasProyecto);
                            enProyecto = false;
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                    }
                });
        }

        function guardar() {

            var listaPolitica = vm.listaPoliticasProyectos;

            vm.ff = [];

            listaPolitica.forEach(ff => {
                if (ff.activado) {
                    vm.ff.push(ff);
                }
            });

            var parametros = {
                ProyectoId: 0,
                bpin: vm.BPIN,
                Politicas: vm.ff
            };

            return focalizacionAjustesServicio.guardarPoliticasTransversalesAjustes(parametros, usuarioDNP)
                .then(function (response) {
                    let exito = response.data;
                    if (exito === '"ok"') {
                     
                        utilidades.mensajeSuccess('Las nuevas políticas se visualizan ahora en la tabla "Políticas transversales asociadas" y en el capítulo "Categorías políticas transversales"', false, function funcionContinuar() {
                            $sessionStorage.close();
                        }, false, 'Los datos se han agregado con éxito');


                      
                    }
                    else {
                        var mensaje = response.data.Mensaje;
                        utilidades.mensajeError(mensaje.substr(mensaje.indexOf(':') + 1), false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
                    //var respuesta = jQuery.parseJSON(response.data)

            //        if (respuesta.StatusCode == "200") {
            //            guardarCapituloModificado();
            //            utilidades.mensajeSuccess("Se han adicionado lineas de información en la parte inferior de la tabla 'Modificar Fuentes'", false, false, false);
            //            //init();
            //            asignaValoresOriginales(2);
            //            vm.permiteEditar = false;
            //            vm.habilitarFinal = false;
            //            vm.disabled = true;
            //            vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

            //            vm.recargaGuardado();

            //            return resumenFuentesServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
            //                function (resp) {
            //                    if (resp.data != null && resp.data != "") {
            //                        vm.validacionGuardado();
            //                    }
            //                }
            //            );
            //        } else {
            //            var ValidacionGuardar = document.getElementById(vm.nombreComponente + "-validacionguardar-error");
            //            if (ValidacionGuardar != undefined) {
            //                var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionguardar-error-mns");
            //                if (ValidacionFFR1Error != undefined) {
            //                    ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
            //                    ValidacionFFR1.classList.remove('hidden');
            //                }
            //            }
            //            utilidades.mensajeError("Error al realizar la operación", false);
            //        }
            //    })
            //    .catch(error => {
            //        if (error.status == 400) {
            //            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
            //            return;
            //        }
            //        utilidades.mensajeError("Error al realizar la operación");
            //    });

        }

        async function _listarRecursosEntidades(idEntidad) {
            var listaRecursos = [];

            return recursosAjustesServicio.obtenerListaRecursos(parametros, idEntidad)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arregloRecursos = jQuery.parseJSON(respuesta.data);
                    arregloRecursos = arregloRecursos.filter(x => x.EntityTypeId == vm.model.idTipoEntidad);

                    for (var ls = 0; ls < arregloRecursos.length; ls++) {
                        var Recurso = {
                            "nombreRecurso": arregloRecursos[ls].Name,
                            "idRecurso": arregloRecursos[ls].Id,
                        }
                        listaRecursos.push(Recurso);
                    }
                    vm.listaRecursos = listaRecursos;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }
    }

    angular.module('backbone').controller('modalAgregarPoliticaTransversalAjustesController', modalAgregarPoliticaTransversalAjustesController);

})();
