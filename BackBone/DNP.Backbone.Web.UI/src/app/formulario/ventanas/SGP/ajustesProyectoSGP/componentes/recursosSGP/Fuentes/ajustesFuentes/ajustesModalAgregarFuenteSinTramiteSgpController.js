(function () {
    'use strict';

    ajustesModalAgregarFuenteSinTramiteSgpController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'recursosAjustesSinTramiteSgpServicio',
        'gestionRecursosSGPServicio',
        'utilidades'
    ];

    function ajustesModalAgregarFuenteSinTramiteSgpController(
        $uibModalInstance,
        $sessionStorage,
        recursosAjustesSinTramiteSgpServicio,
        gestionRecursosSGPServicio,
        utilidades
    ) {
        var vm = this;
        vm.init = init;
        vm.cerrar = $sessionStorage.close;
        vm.guardar = guardar;
        vm.cambiarTipoEntidad = cambiarTipoEntidad;
        vm.cambiarRecurso = cambiarRecurso;
        vm.recursoSeleccionado = entidadSeleccionado;
        vm.cambioSector = cambioSector;
        vm.cambioDepartamento = cambioDepartamento;
        vm.cambioComunidad = cambioComunidad;
        vm.RespuestaAgregar = null;
        vm.guardo = false;

        vm.options = [];

        //vm.arreglolistaEntidades = [];

        vm.recursoVisible = true;
        vm.nombrePrivadoRecurso = false;
        vm.RecursoVisible = true;
        vm.EntidadVisible = true;
        vm.sectorVisible = false;
        vm.municipioVisible = false;
        vm.comunidadVisible = false;

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

        function cambiarTipoEntidad() {
            _limpiarListas(1);
            if (!vm.model.idTipoEntidad)
                return;
            entidadSeleccionado();
        }

        function cambiarRecurso() {
            _limpiarListas(2);
            if (!vm.model.Entidad.idEntidad)
                return;
            vm.model.Entidades = [];
            //_listarRecursosEntidades(vm.model.Entidad.idEntidad);
            _listarRecursosEntidadesSgp(vm.model.Entidad.idEntidad, vm.model.idTipoEntidad);
        }

        function cambioSector() {

            _limpiarListas(4);
            var sectorSeleccionado = vm.model.Sector;
            var listaEntidades = [];
            return recursosAjustesSinTramiteSgpServicio.obtenerListaEntidades(parametros, 1)
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
                    vm.listaEntidades = listaEntidades.sort(GetSortOrder("codigo"));

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }

        function entidadSeleccionado() {

            switch (vm.model.idTipoEntidad) {
                case 1:
                    _listarTiposEntidades(0);
                    vm.sectorVisible = true;
                    vm.RecursoVisible = true;
                    obtenerSectores();

                    vm.nombrePrivadoRecurso = false;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    break;
                case 4:
                    obtenerDepartamentos();
                    vm.municipioVisible = true;
                    vm.EntidadVisible = true;
                    vm.RecursoVisible = true;

                    vm.nombrePrivadoRecurso = false;
                    vm.sectorVisible = false;
                    vm.comunidadVisible = false;
                    break;
                case 5:
                    vm.nombrePrivadoRecurso = true;
                    vm.sectorVisible = false;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    vm.EntidadVisible = false;
                    vm.RecursoVisible = false;

                    break;
                //case 11:
                //    _listarTiposEntidades(0);
                //    vm.comunidadVisible = true;
                //    vm.nombrePrivadoRecurso = false;
                //    vm.sectorVisible = false;
                //    vm.municipioVisible = false;
                //    vm.RecursoVisible = true;
                //    obtenerComunidades();
                //    break;
                default:
                    vm.model.Entidades = [];
                    _listarTiposEntidades(vm.model.idTipoEntidad);
                    vm.EntidadVisible = true;
                    vm.nombrePrivadoRecurso = false;
                    vm.RecursoVisible = true;
                    vm.sectorVisible = false;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    break;
            }

        }

        function obtenerEtapas() {

            recursosAjustesSinTramiteSgpServicio.obtenerListaEtapas(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        let operacion = localStorage.getItem("operacion");
                        let preinversion = localStorage.getItem("preinversion");
                        let inversion = localStorage.getItem("inversion");
                        vm.Etapas = response.data;
                        let etapasfiler = [];
                        if (operacion === 'true') etapasfiler.push(3);
                        if (preinversion === 'true') etapasfiler.push(1);
                        if (inversion === 'true') etapasfiler.push(2);
                        vm.Etapas = vm.Etapas.filter(element => etapasfiler.includes(element.Id));
                });
                });
        }

        function obtenerListaTipoEntidad() {

            recursosAjustesSinTramiteSgpServicio.obtenerListaTipoEntidad(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        var listaTiposEntidades= [];
                        var arreglolistaTipoEntidades = response.data;
                        for (var ls = 0; ls < arreglolistaTipoEntidades.length; ls++) {
                            if ((arreglolistaTipoEntidades[ls].Name == "Departamentos") || (arreglolistaTipoEntidades[ls].Name == "Municipios")    ||
                                (arreglolistaTipoEntidades[ls].Name == "Privadas")      || (arreglolistaTipoEntidades[ls].Name == "Entidades Presupuesto Nacional - PGN") ||
                                (arreglolistaTipoEntidades[ls].Name == "Grupos étnicos - pueblos y comunidades indígenas")) {
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
            recursosAjustesSinTramiteSgpServicio.obtenerSectores(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        vm.listaSectores = response.data;
                    });
                });
        }

        function obtenerDepartamentos() {

            var listaDepartamentos = [];

            return recursosAjustesSinTramiteSgpServicio.obtenerListaEntidades(parametros, 3)
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

        function obtenerComunidades() {

            var listaComunidades = [];

            return recursosAjustesSinTramiteSgpServicio.obtenerListaEntidades(parametros, 3)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidadesComunidades = jQuery.parseJSON(respuesta.data);                    
                    arreglolistaEntidadesComunidades = arreglolistaEntidadesComunidades.filter(item => item.EntityTypeId === 11 && (item.ParentId === null || item.ParentId === undefined));
                    for (var ls = 0; ls < arreglolistaEntidadesComunidades.length; ls++) {
                        var entidad = {
                            "Nombre": arreglolistaEntidadesComunidades[ls].Name,
                            "Id": arreglolistaEntidadesComunidades[ls].Id,
                        }
                        listaComunidades.push(entidad);
                    }
                    vm.listaComunidades = listaComunidades;

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las Comunidades");
                });

        }

        function cambioDepartamento() {

            _limpiarListas(3);
            var listaEntidades = [];
            return recursosAjustesSinTramiteSgpServicio.obtenerListaEntidades(parametros, vm.model.Departamento.Id)
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

        function cambioComunidad() {
            var listaEntidades = [];
            return recursosAjustesSinTramiteSgpServicio.obtenerListaEntidades(parametros, vm.model.Comunidad.Id)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidades = arreglolistaEntidades.filter(item => item.ParentId == vm.model.Comunidad.Id)
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

            if (!vm.model.Entidad && vm.nombrePrivadoRecurso == false) {
                utilidades.mensajeError("Verifique la Entidad Financiador(a).", false); return false;
            }

            if (!vm.model.Recurso && vm.nombrePrivadoRecurso == false) {
                utilidades.mensajeError("Verifique el Tipo Recurso.", false); return false;
            }


            if (vm.nombrePrivadoRecurso == true) {
                var params = {
                    ProyectoId: $uibModalInstance.proyectoId,
                    BPIN: vm.BPIN,
                    CR: 3,

                    FuentesFinanciacionAgregar: [{
                        FuenteId: null,

                        IdGrupoRecurso: 4,
                        CodigoGrupoRecurso: 4,
                        NombreGrupoRecurso: 'Propios',

                        IdTipoEntidad: vm.model.idTipoEntidad,
                        CodigoTipoEntidad: vm.model.idTipoEntidad,
                        NombreTipoEntidad: 'Privadas',

                        IdEntidad: null,
                        CodigoEntidad: null,
                        NombreEntidad: vm.nombrePrivado,

                        IdTipoRecurso: 3,
                        CodigoTipoRecurso: 3,
                        NombreTipoRecurso: vm.nombrePrivado,

                        IdEtapa: vm.model.idEtapa,
                        NombreEtapa: vm.model.idEtapa
                    }]
                };
            }
            else
                var params = {
                    ProyectoId: $uibModalInstance.proyectoId,
                    BPIN: vm.BPIN,
                    CR: 3,

                    FuentesFinanciacionAgregar: [{
                        FuenteId: null,

                        IdGrupoRecurso: vm.model.Recurso.idRecurso,
                        CodigoGrupoRecurso: vm.model.Recurso.idRecurso,
                        NombreGrupoRecurso: vm.model.Recurso.nombreRecurso,

                        IdTipoEntidad: vm.model.idTipoEntidad,
                        CodigoTipoEntidad: vm.model.idTipoEntidad,
                        NombreTipoEntidad: "", //vm.model.nombreTipoEntidad,

                        IdEntidad: vm.model.Entidad.idEntidad,
                        CodigoEntidad: vm.model.Entidad.idEntidad,
                        NombreEntidad: vm.model.Entidad.nombreEntidad,

                        IdTipoRecurso: vm.model.Recurso.idRecurso,
                        CodigoTipoRecurso: vm.model.Recurso.idRecurso,
                        NombreTipoRecurso: vm.model.Recurso.nombreRecurso,

                        IdEtapa: vm.model.idEtapa,
                        NombreEtapa: vm.model.idEtapa
                    }]
                };

            recursosAjustesSinTramiteSgpServicio.agregarFuentesFinanciacionProyecto(params, usuarioDNP, $uibModalInstance.idInstancia, $uibModalInstance.idAccion, $uibModalInstance.idNivel)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        console.log(params);
                        vm.options.push(params);
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        $sessionStorage.close(vm.options);
                        /*$uibModalInstance.close(vm.options);*/
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación - Valide si la fuente que está intentando guardar ya está en el proyecto.", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación - Valide si la fuente que está intentando guardar ya está en el proyecto");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación - Valide si la fuente que está intentando guardar ya está en el proyecto.");
                });

        }

        async function _listarTiposEntidades(tipoEntidad) {

            var listaEntidades = [];
            return recursosAjustesSinTramiteSgpServicio.obtenerListaEntidades(parametros, tipoEntidad)
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

        async function _listarRecursosEntidades(idEntidad) {
            var listaRecursos = [];
            if (idEntidad == undefined || idEntidad == 0) {
                return;
            }
            return recursosAjustesSinTramiteSgpServicio.obtenerListaRecursos(idEntidad, vm.model.idTipoEntidad)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;
                    var arregloRecursos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));                    

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

        async function _listarRecursosEntidadesSgp(entityTypeCatalogId, entityType) {
            var listaRecursosSgp = [];

            return gestionRecursosSGPServicio.ObtenerListaTiposRecursosxEntidadSgp(parametros, entityTypeCatalogId, entityType)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;
                    var arregloRecursos = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arregloRecursos.length; ls++) {
                        var Recurso = {
                            "nombreRecurso": arregloRecursos[ls].Name,
                            "idRecurso": arregloRecursos[ls].Id,
                        }
                        listaRecursosSgp.push(Recurso);
                    }
                    vm.listaRecursos = listaRecursosSgp;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }

        function GetSortOrder(prop) {
            return function (a, b) {
                if (a[prop] > b[prop]) {
                    return 1;
                } else if (a[prop] < b[prop]) {
                    return -1;
                }
                return 0;
            }
        }

        function _limpiarListas(idLista) {
            switch (idLista) {
                case 1:
                    vm.model.Sector = null;
                    vm.listaSectores = [];
                    vm.model.Comunidad = null;
                    vm.listaComunidades = [];
                    vm.model.Departamento = null;
                    vm.listaDepartamento = [];
                    vm.model.Entidad = null;
                    vm.listaEntidades = [];
                    vm.model.Recurso = null;
                    vm.listaRecursosSgp = [];
                    vm.nombrePrivado = "";
                    break;
                case 2:
                    vm.model.Comunidad = null;
                    vm.listaComunidades = [];
                    vm.model.Recurso = null;
                    vm.listaRecursosSgp = [];
                    vm.nombrePrivado = "";
                    break;
                case 3:
                    vm.model.Comunidad = null;
                    vm.listaComunidades = [];
                    vm.model.Entidad = null;
                    vm.listaEntidades = [];
                    vm.model.Recurso = null;
                    vm.listaRecursosSgp = [];
                    vm.nombrePrivado = "";
                    break;
                case 4:
                    vm.model.Comunidad = null;
                    vm.listaComunidades = [];
                    vm.model.Entidad = null;
                    vm.listaEntidades = [];
                    vm.model.Recurso = null;
                    vm.listaRecursosSgp = [];
                    vm.nombrePrivado = "";
                    break;
            }
        }
    }

    angular.module('backbone').controller('ajustesModalAgregarFuenteSinTramiteSgpController', ajustesModalAgregarFuenteSinTramiteSgpController);

})();
