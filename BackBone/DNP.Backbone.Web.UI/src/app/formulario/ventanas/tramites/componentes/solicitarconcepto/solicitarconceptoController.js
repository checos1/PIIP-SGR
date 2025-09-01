(function () {
    'use strict';

    solicitarconceptoController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage',
        'solicitarconceptoServicio',
        'trasladosServicio',
        'constantesBackbone',
        '$routeParams',
        'servicioResumenDeProyectos',
        'uiGridConstants',
        '$timeout',
        '$location'
    ];



    function solicitarconceptoController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage,
        solicitarconceptoServicio,
        trasladosServicio,
        constantesBackbone,
        $routeParams,
        servicioResumenDeProyectos,
        uiGridConstants,
        $timeout,
        $location
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.solicitar = solicitar;
        vm.existeconcepto = false;
        vm.lbldirecciontecnica = "";
        vm.lblsubdirecciontecnica = "";
        vm.lblanalista = "";
        vm.cumple = false;
        vm.salir = salir;


        /*---*/
        vm.cargaranalistas = cargaranalistas;
        vm.cargarsubdirecciontecnica = cargarsubdirecciontecnica;
        vm.cargarsubdirecciontecnicarecuperar = cargarsubdirecciontecnicarecuperar;
        vm.direcciontecnicas = [];
        vm.subdireccionestecnicas = [];
        vm.analistas = [];
        vm.filtro = {
            direccionestecnica: "",
            subdireccionestecnica: "",
            analista: "",
            parentId: "",
            get seleccionado() {
                if (this.analista !== "") return this.analista;
                if (this.subdireccionestecnica !== "") return this.subdireccionestecnica;
                if (this.direccionestecnica !== "") return this.direccionestecnica;
                return "";
            },
            limpiar: function () {
                this.analistas = "";
                this.subdireccionestecnicas = "";
                this.direccionestecnicas = "";
                vm.subdireccionestecnicas = [];
                vm.analistas = [];
            }
        }
        /*----*/



        vm.peticion = {
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoProyecto,
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdFiltro: vm.EntityTypeCatalogOptionId,
        };
        vm.concepto = {
            Id: 0,
            EntityTypeCatalogOptionId: 0,
            IdUsuarioDNP: "",
            ParentId: 0
        };

        vm.isdisabled = true;
        vm.ListaDireccionTecnica = [];
        vm.ListaSubDireccionTecnica = [];

        function init() {
            if ($sessionStorage.concepto.length > 0 && $sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id > 0 && $sessionStorage.concepto[$sessionStorage.concepto.length - 1].Activo == true && $sessionStorage.concepto[$sessionStorage.concepto.length - 1].Enviado == false) {
                CargarDireccionTecnicarecuperar();
                vm.existeconcepto = true;
            }
            else {
                CargarDireccionTecnica();
                vm.existeconcepto = false;
            }
        }



        function CargarDireccionTecnicarecuperar() {
            solicitarconceptoServicio.ObtenerDireccionTecnica(vm.peticion)
                .then(resultado => {
                    vm.direcciontecnicas = resultado.data;
                    vm.direcciontecnicas.parentId = resultado.data[0].ParentId;
                    if ($sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id > 0) {
                        vm.direcciontecnicas.forEach(dt => {
                            if (dt.EntityTypeCatalogOptionId == $sessionStorage.concepto[$sessionStorage.concepto.length - 1].EntityTypeCatalogOptionId) {
                                vm.lbldirecciontecnica = dt.Name;
                            }
                        });
                    }
                    cargarsubdirecciontecnicarecuperar();
                })
        }

        function cargarsubdirecciontecnicarecuperar() {

            vm.peticion.IdFiltro = $sessionStorage.concepto[$sessionStorage.concepto.length - 1].ParentId;
            solicitarconceptoServicio.ObtenerSubDireccionTecnica(vm.peticion)
                .then(resultado => {
                    vm.subdireccionestecnicas = resultado.data;
                    if ($sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id > 0) {
                        vm.subdireccionestecnicas.forEach(dt => {
                            if (dt.EntityTypeCatalogOptionId == $sessionStorage.concepto[$sessionStorage.concepto.length - 1].EntityTypeCatalogOptionId) {
                                vm.lblsubdirecciontecnica = dt.Name;
                            }
                        });
                        cargaranalistasrecuperar();
                    }
                })
        }

        function cargaranalistasrecuperar() {
            vm.peticion.IdFiltro = $sessionStorage.concepto[$sessionStorage.concepto.length - 1].EntityTypeCatalogOptionId;
            solicitarconceptoServicio.ObtenerAnalistasSubDireccionTecnica(vm.peticion)
                .then(resultado => {
                    vm.analistas = resultado.data;
                    if ($sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id > 0) {
                        vm.analistas.forEach(dt => {
                            if (dt.IdUsuarioDnp == $sessionStorage.concepto[$sessionStorage.concepto.length - 1].IdUsuarioDNP) {
                                vm.lblanalista = dt.Nombre;
                            }
                        });
                    }
                })
        }


        function CargarDireccionTecnica() {
            solicitarconceptoServicio.ObtenerDireccionTecnica(vm.peticion)
                .then(resultado => {
                    vm.direcciontecnicas = resultado.data;
                    vm.direcciontecnicas.parentId = resultado.data.ParentId;
                })
        }

        function cargarsubdirecciontecnica(macroproceso) {
            vm.peticion.IdFiltro = macroproceso;
            solicitarconceptoServicio.ObtenerSubDireccionTecnica(vm.peticion)
                .then(resultado => {
                    vm.subdireccionestecnicas = resultado.data;
                })
        }

        function cargaranalistas(macroproceso) {
            vm.peticion.IdFiltro = macroproceso;
            solicitarconceptoServicio.ObtenerAnalistasSubDireccionTecnica(vm.peticion)
                .then(resultado => {
                    vm.analistas = resultado.data;
                })
        }

        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));

        function solicitar() {
            if ($sessionStorage.concepto.length > 0 && $sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id > 0 && $sessionStorage.concepto[$sessionStorage.concepto.length - 1].Activo == true) {
                if ($sessionStorage.concepto[$sessionStorage.concepto.length - 1].Enviado == true) {
                    vm.peticion.IdFiltro = '{"Id": 0,"TramiteId": ' + $sessionStorage.TramiteId +
                        ',"IdUsuarioDNP": "' + vm.filtro.seleccionado +
                        '","EntityTypeCatalogOptionId":' + vm.filtro.subdireccionestecnica +
                        ',"Activo": true, "FechaCreacion": "' + vm.fechaHoy +
                        '","CreadoPor": "' + vm.peticion.IdUsuario +
                        '","FechaModificacion": "' + vm.fechaHoy +
                        '","ModificadoPor": "' + vm.peticion.IdUsuario + '","Enviado": false' +
                        ',"ParentId":' + vm.filtro.direccionestecnica + '}';
                }
                else {
                    solicitarconceptoServicio.eliminarPermisos($sessionStorage.concepto[$sessionStorage.concepto.length - 1].IdUsuarioDNP, $sessionStorage.TramiteId, 'TEC','00000000-0000-0000-0000-000000000000');

                    vm.peticion.IdFiltro = '{"Id": ' + $sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id + ', "TramiteId": ' + $sessionStorage.TramiteId +
                        ',"IdUsuarioDNP": "' + $sessionStorage.concepto[$sessionStorage.concepto.length - 1].IdUsuarioDNP +
                        '","EntityTypeCatalogOptionId":' + $sessionStorage.concepto[$sessionStorage.concepto.length - 1].EntityTypeCatalogOptionId +
                        ',"Activo": "' + !$sessionStorage.concepto[$sessionStorage.concepto.length - 1].Activo + '", "FechaCreacion": "' + vm.fechaHoy +
                        '","CreadoPor": "' + vm.peticion.IdUsuario +
                        '","FechaModificacion": "' + vm.fechaHoy +
                        '","ModificadoPor": "' + vm.peticion.IdUsuario + '","Enviado": false' +
                        ',"ParentId":' + $sessionStorage.concepto[$sessionStorage.concepto.length - 1].ParentId + '}';
                }
                solicitarconceptoServicio.SolicitarConcepto(vm.peticion)
                    .then(function (response) {
                        if (response.data) {
                            RegistrarPermisosAccionPorUsuario();
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            if ($sessionStorage.concepto[$sessionStorage.concepto.length - 1].Enviado == true) {
                                trasladosServicio.CrearReasignacionRadicadOrfeo(vm.peticion.IdUsuario, vm.filtro.seleccionado, $sessionStorage.TramiteId);
                            }
                            else {
                                var analista = vm.analistas.filter(analista => analista.Nombre == vm.lblanalista)[0];
                                trasladosServicio.CrearReasignacionRadicadOrfeo(analista.IdUsuarioDnp, vm.peticion.IdUsuario, $sessionStorage.TramiteId);
                            }
                            vm.versolicitarconcepto = true;
                            vm.callback({ arg: true });
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                            vm.callback({ arg: false });
                        }
                    });
            }
            else {
                ValidarSalicitarConcepto();
                if (vm.cumple == true) {
                    vm.peticion.IdFiltro = '{"Id": 0,"TramiteId": ' + $sessionStorage.TramiteId +
                        ',"IdUsuarioDNP": "' + vm.filtro.seleccionado +
                        '","EntityTypeCatalogOptionId":' + vm.filtro.subdireccionestecnica +
                        ',"Activo": true, "FechaCreacion": "' + vm.fechaHoy +
                        '","CreadoPor": "' + vm.peticion.IdUsuario +
                        '","FechaModificacion": "' + vm.fechaHoy +
                        '","ModificadoPor": "' + vm.peticion.IdUsuario + '","Enviado": false' +
                        ',"ParentId":' + vm.filtro.direccionestecnica + '}';


                    solicitarconceptoServicio.SolicitarConcepto(vm.peticion)
                        .then(function (response) {
                            if (response.data) {
                                RegistrarPermisosAccionPorUsuario();
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                trasladosServicio.CrearReasignacionRadicadOrfeo(vm.peticion.IdUsuario, vm.filtro.seleccionado);
                                vm.versolicitarconcepto = false;
                                vm.callback({ arg: false });
                            } else {
                                swal('', "Error al realizar la operación", 'error');
                                vm.callback({ arg: true });
                            }
                        });
                }
                else {
                    utilidades.mensajeError('El formulario no esta diligenciado en su totalidad');
                }
            }
        }

        function ValidarSalicitarConcepto() {
            vm.cumple = true;
            if (vm.filtro.direccionestecnica == null || vm.filtro.direccionestecnica == '') {
                vm.cumple = false;
            }
            if (vm.filtro.subdireccionestecnica == null || vm.filtro.subdireccionestecnica == '') {
                vm.cumple = false;
            }
            if (vm.filtro.analista == null || vm.filtro.analista == '') {
                vm.cumple = false;
            }
        }

        function RegistrarPermisosAccionPorUsuario() {

            vm.listadoUsuarios = [{
                IdUsuarioDNP: vm.filtro.seleccionado,
                NombreUsuario: vm.peticion.IdUsuario,
                IdRol: constantesBackbone.idRControlPosteriorDireccionesTecnicas,
                NombreRol: 'R_Concepto Tecnico',
                IdEntidad: 'Concepto Tecnico',
                NombreEntidad: 'Concepto Tecnico',
                IdEntidadMGA: $sessionStorage.idEntidad,
            }];

            vm.RegistrarPermisosAccionDto = {
                ObjetoNegocioId: $sessionStorage.TramiteId,
                IdAccion: $sessionStorage.IdAccion,
                IdInstancia: $sessionStorage.idInstancia,
                EntityTypeCatalogOptionId: $sessionStorage.idEntidad,
                listadoUsuarios: vm.listadoUsuarios,
            };

            trasladosServicio.RegistrarPermisosAccionPorUsuario(vm.RegistrarPermisosAccionDto).then(function (response) {

            });
        }

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function salir() {
            vm.versolicitarconcepto = false;
            vm.callback({ arg: false });
        }

    }

    angular.module('backbone').component('solicitarConcepto', {

        templateUrl: "src/app/formulario/ventanas/tramites/componentes/solicitarconcepto/solicitarconcepto.html",
        controller: solicitarconceptoController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&'
        }
    });


})();