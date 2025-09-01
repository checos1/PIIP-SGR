(function () {
    'use strict';

    informacionDetalladaSgrModalAgregarFuenteController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'recursosAjustesServicio',
        'operacionesCreditoSgrServicio',
        'utilidades'
    ];

    function informacionDetalladaSgrModalAgregarFuenteController(
        $uibModalInstance,
        $sessionStorage,
        recursosAjustesServicio,
        operacionesCreditoSgrServicio,
        utilidades
    ) {
        var vm = this;
        vm.init = init;
        vm.cerrar = $sessionStorage.close;
        vm.guardar = guardar;
        vm.cambiarEtapa = cambiarEtapa;
        vm.cambiarTipoEntidad = cambiarTipoEntidad;
        vm.cambiarRecurso = cambiarRecurso;
        vm.recursoSeleccionado = entidadSeleccionado;
        vm.cambioSector = cambioSector;
        vm.cambioDepartamento = cambioDepartamento;
        //vm.cambioComunidad = cambioComunidad;
        vm.RespuestaAgregar = null;
        vm.guardo = false;

        vm.FuenteAdicional = {};

        vm.arreglolistaEntidades = [];

        vm.recursoVisible = true;
        vm.nombrePrivadoRecurso = false;
        vm.RecursoVisible = true;
        vm.EntidadVisible = true;
        vm.sectorVisible = true;

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
            obtenerEtapas();
            obtenerListaTipoEntidad();
            /*ListaEntidades(); ObtenerSectores();*/
        }

        function cambiarEtapa() {
            if (!vm.model.idEtapa)
                return;
            vm.model.nombreEtapa = vm.Etapas.find(x => x.Id == vm.model.idEtapa).Name;
            entidadSeleccionado();
        }

        function cambiarTipoEntidad() {
            if (!vm.model.idTipoEntidad)
                return;
            vm.model.nombreTipoEntidad = vm.tiposEntidades.find(x => x.Id == vm.model.idTipoEntidad).Name;
            entidadSeleccionado();
        }

        function cambiarRecurso() {
            if (!vm.model.Entidad.idEntidad)
                return;
            vm.model.Entidades = [];
            _listarRecursosEntidades(vm.model.Entidad.idEntidad, 4, false);
        }

        function cambioSector() {
            var sectorSeleccionado = vm.model.Sector;
            var listaEntidades = [];
            return recursosAjustesServicio.obtenerListaEntidades(parametros, 1)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidades = arreglolistaEntidades.filter(item => item.ParentId == sectorSeleccionado.Id)
                    for (var ls = 0; ls < arreglolistaEntidades.length; ls++) {
                        var entidad = {
                            "nombreEntidad": arreglolistaEntidades[ls].Name,
                            "idEntidad": arreglolistaEntidades[ls].Id,
                        }
                        listaEntidades.push(entidad);
                    }
                    vm.listaEntidades = listaEntidades;

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }

        function entidadSeleccionado() {

            switch (vm.model.idTipoEntidad) {
                //Entidades Presupuesto Nacional - PGN	1	
                case 1:
                    _listarTiposEntidades(0);
                    _listarRecursosEntidades(0, 0, false);
                    obtenerSectores();
                    vm.sectorVisible = true;
                    vm.RecursoVisible = true;
                    vm.nombrePrivadoRecurso = false;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    break;
                //Municipios	                        4
                case 4:
                    _listarTiposEntidades(0);
                    _listarRecursosEntidades(0, 0, false);
                    obtenerDepartamentos();
                    vm.sectorVisible = false;
                    vm.EntidadVisible = true;
                    vm.RecursoVisible = true;
                    vm.municipioVisible = true;
                    vm.nombrePrivadoRecurso = false;
                    vm.comunidadVisible = false;
                    break;
                //Privadas	                            5
                case 5:
                    vm.sectorVisible = false;
                    vm.EntidadVisible = false;
                    vm.RecursoVisible = false;
                    vm.nombrePrivadoRecurso = true;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    break;
                //Minoría Étnica – Resguardo	        7	
                //case 10:
                //    _listarTiposEntidades(0);
                //    _listarRecursosEntidades(0, 0, false);

                //    vm.sectorVisible = false;
                //    vm.RecursoVisible = true;

                //    vm.comunidadVisible = true;
                //    vm.nombrePrivadoRecurso = false;
                //    vm.municipioVisible = false;
                //    obtenerComunidades();
                //    break;
                //Departamentos	                    3
                default:
                    vm.model.Entidades = [];
                    _listarTiposEntidades(vm.model.idTipoEntidad);
                    //_listarRecursosEntidades(vm.IdTipoRecurso, 4, false);
                    _listarRecursosEntidades(0, 0, false);
                    vm.sectorVisible = false;
                    vm.EntidadVisible = true;
                    vm.RecursoVisible = true;
                    vm.nombrePrivadoRecurso = false;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    break;
            }
        }

        function obtenerEtapas() {

            recursosAjustesServicio.obtenerListaEtapas(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        vm.Etapas = response.data;
                    });
                });
        }

        function obtenerListaTipoEntidad() {

            recursosAjustesServicio.obtenerListaTipoEntidad(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        var listaTiposEntidades= [];
                        var arreglolistaTipoEntidades = response.data;
                        for (var ls = 0; ls < arreglolistaTipoEntidades.length; ls++) {
                            if ((arreglolistaTipoEntidades[ls].Name == "Departamentos") || (arreglolistaTipoEntidades[ls].Name == "Municipios")    ||
                                (arreglolistaTipoEntidades[ls].Name == "Privadas")      || (arreglolistaTipoEntidades[ls].Name == "Entidades Presupuesto Nacional - PGN") /*||*/
                                /*(arreglolistaTipoEntidades[ls].Name == "Minoría Étnica – Resguardo")*/) {
                                var entidad = {
                                    "Name": arreglolistaTipoEntidades[ls].Name,
                                    "Id": arreglolistaTipoEntidades[ls].Id,
                                }
                                listaTiposEntidades.push(entidad);
                            }
                        }

                        vm.tiposEntidades = listaTiposEntidades;
                    });
                });
        }

        function obtenerSectores() {
            recursosAjustesServicio.obtenerSectores(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        vm.listaSectores = response.data;
                    });
                });
        }

        function obtenerDepartamentos() {

            var listaDepartamentos = [];

            return recursosAjustesServicio.obtenerListaEntidades(parametros, 3)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidadesDepartamentos = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidadesDepartamentos = arreglolistaEntidadesDepartamentos.filter(item => item.EntityTypeId == 3)
                    for (var ls = 0; ls < arreglolistaEntidadesDepartamentos.length; ls++) {
                        var entidad = {
                            "Nombre": arreglolistaEntidadesDepartamentos[ls].Name,
                            "Id": arreglolistaEntidadesDepartamentos[ls].Id,
                        }
                        listaDepartamentos.push(entidad);
                    }
                    vm.listaDepartamento = listaDepartamentos;

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }

        //function obtenerComunidades() {

        //    var listaComunidades = [];

        //    return recursosAjustesServicio.obtenerListaEntidades(parametros, 3)
        //        .then(respuesta => {
        //            if (!respuesta.data)
        //                return;

        //            var arreglolistaEntidadesComunidades = jQuery.parseJSON(respuesta.data);
        //            arreglolistaEntidadesComunidades = arreglolistaEntidadesComunidades.filter(item => item.EntityTypeId == 10 && item.ParentId == null)
        //            for (var ls = 0; ls < arreglolistaEntidadesComunidades.length; ls++) {
        //                var entidad = {
        //                    "Nombre": arreglolistaEntidadesComunidades[ls].Name,
        //                    "Id": arreglolistaEntidadesComunidades[ls].Id,
        //                }
        //                listaComunidades.push(entidad);
        //            }
        //            vm.listaComunidades = listaComunidades;

        //        })
        //        .catch(error => {
        //            console.log(error);
        //            toastr.error("Hubo un error al cargar las Comunidades");
        //        });

        //}

        function cambioDepartamento() {

            var listaEntidades = [];
            return recursosAjustesServicio.obtenerListaEntidades(parametros, vm.model.Departamento.Id)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidades = arreglolistaEntidades.filter(item => item.ParentId == vm.model.Departamento.Id)
                    for (var ls = 0; ls < arreglolistaEntidades.length; ls++) {
                        var entidad = {
                            "nombreEntidad": arreglolistaEntidades[ls].Name,
                            "idEntidad": arreglolistaEntidades[ls].Id,
                        }
                        listaEntidades.push(entidad);
                    }
                    vm.listaEntidades = listaEntidades;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });

        }

        //function cambioComunidad() {
        //    var listaEntidades = [];
        //    return recursosAjustesServicio.obtenerListaEntidades(parametros, vm.model.Comunidad.Id)
        //        .then(respuesta => {
        //            if (!respuesta.data)
        //                return;

        //            var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
        //            arreglolistaEntidades = arreglolistaEntidades.filter(item => item.ParentId == vm.model.Comunidad.Id)
        //            for (var ls = 0; ls < arreglolistaEntidades.length; ls++) {
        //                var entidad = {
        //                    "nombreEntidad": arreglolistaEntidades[ls].Name,
        //                    "idEntidad": arreglolistaEntidades[ls].Id,
        //                }
        //                listaEntidades.push(entidad);
        //            }
        //            vm.listaEntidades = listaEntidades;
        //        })
        //        .catch(error => {
        //            console.log(error);
        //            toastr.error("Hubo un error al cargar las entidad");
        //        });
        //}

        function guardar() {

            if (!vm.model.idEtapa) {
                utilidades.mensajeError("Verifique la etapa.", false); return false;
            }

            if (!vm.model.idTipoEntidad) {
                utilidades.mensajeError("Verifique el Tipo Entidad.", false); return false;
            }

            if (!vm.model.Sector && vm.sectorVisible == true) {
                utilidades.mensajeError("Verifique el Sector.", false); return false;
            }

            if (!vm.model.Entidad /*&& vm.nombrePrivadoRecurso == false*/) {
                utilidades.mensajeError("Verifique la Entidad Financiador(a).", false); return false;
            }

            if (!vm.model.Recurso /*&& vm.nombrePrivadoRecurso == false*/) {
                utilidades.mensajeError("Verifique el Tipo Recurso.", false); return false;
            }

            var resultado = {
                "FuenteAdicionalCreditoId": 0,
                "IdEtapa": vm.model.idEtapa,
                "Etapa": vm.model.nombreEtapa,
                "IdTipoEntidad": vm.model.idTipoEntidad,
                "TipoEntidad": vm.model.nombreTipoEntidad,
                "IdEntidad": vm.model.Entidad.idEntidad,
                "Entidad": vm.model.Entidad.nombreEntidad,
                "IdTipoRecurso": vm.model.Recurso.idRecurso,
                "TipoRecurso": vm.model.Recurso.nombreRecurso,
                "CostoFinanciero": 0,
                "CostoPatrimonio": 0
            }

            vm.FuenteAdicional = resultado;
            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
            $sessionStorage.close(vm.FuenteAdicional);

        }

        async function _listarTiposEntidades(tipoEntidad) {

            var listaEntidades = [];
            return recursosAjustesServicio.obtenerListaEntidades(parametros, tipoEntidad)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arreglolistaEntidades.length; ls++) {
                        if (tipoEntidad == arreglolistaEntidades[ls].EntityTypeId) {
                            var entidad = {
                                "nombreEntidad": arreglolistaEntidades[ls].Name,
                                "idEntidad": arreglolistaEntidades[ls].Id,
                            }

                            listaEntidades.push(entidad);
                        }
                    }
                    vm.listaEntidades = listaEntidades;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }

        async function _listarRecursosEntidades(idEntidad, idTipoRecurso, incluir) {
            var listaRecursos = [];

            return operacionesCreditoSgrServicio.obtenerTiposRecursosEntidadPorGrupoRecursos(idEntidad, idTipoRecurso, incluir)
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

    angular.module('backbone').controller('informacionDetalladaSgrModalAgregarFuenteController', informacionDetalladaSgrModalAgregarFuenteController);

})();
