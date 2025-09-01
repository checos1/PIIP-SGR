(function () {
    'use strict';

    modalAgregarFuenteController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'gestionRecursosServicio',
        'utilidades',
        'constantesBackbone'
    ];

    function modalAgregarFuenteController(
        $uibModalInstance,
        $sessionStorage,
        gestionRecursosServicio,
        utilidades,
        constantesBackbone
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

        //vm.arreglolistaEntidades = [];

        vm.recursoVisible = true;
        vm.nombrePrivadoRecurso = false;
        vm.RecursoVisible = true;
        vm.EntidadVisible = true;
        vm.sectorVisible = false;
        vm.municipioVisible = false;
        vm.comunidadVisible = false;
        vm.seccionCapitulo = null;
        vm.nombreComponente = "recursosGr";

        vm.proyectoId = $uibModalInstance.InstanciaSeleccionada.ProyectoId;

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
            //  ObtenerSeccionCapitulo();
            /*ListaEntidades(); ObtenerSectores();*/
        }

        function cambiarTipoEntidad() {
            if (!vm.model.idTipoEntidad)
                return;
            entidadSeleccionado();
        }

        function cambiarRecurso() {
            if (!vm.model.Entidad.idEntidad)
                return;
            vm.model.Entidades = [];
            _listarRecursosEntidades(vm.model.Entidad.idEntidad);
        }

        function cambioSector() {
            var sectorSeleccionado = vm.model.Sector;
            var listaEntidades = [];
            return gestionRecursosServicio.obtenerListaEntidades(parametros, 1)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidades = arreglolistaEntidades.filter(item => item.ParentId == sectorSeleccionado.Id)
                    for (var ls = 0; ls < arreglolistaEntidades.length; ls++) {
                        var entidad = {
                            "nombreEntidad": arreglolistaEntidades[ls].Name,
                            "idEntidad": arreglolistaEntidades[ls].Id,
                            "codigo": arreglolistaEntidades[ls].Code
                        }
                        listaEntidades.push(entidad);
                    }
                    //vm.listaEntidades = listaEntidades.sortby("");
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
                    //_listarRecursosEntidades(0);
                    vm.sectorVisible = true;
                    vm.RecursoVisible = true;
                    obtenerSectores();

                    vm.nombrePrivadoRecurso = false;
                    vm.municipioVisible = false;
                    vm.comunidadVisible = false;
                    break;
                case 4:
                    //_listarTiposEntidades(0);
                    //_listarRecursosEntidades(0);
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
                case 10:
                    _listarTiposEntidades(0);
                    // _listarRecursosEntidades(0);

                    vm.comunidadVisible = true;
                    vm.nombrePrivadoRecurso = false;
                    vm.sectorVisible = false;
                    vm.municipioVisible = false;
                    vm.RecursoVisible = true;
                    obtenerComunidades();
                    break;
                default:
                    vm.model.Entidades = [];
                    _listarTiposEntidades(vm.model.idTipoEntidad);
                    //_listarRecursosEntidades(vm.IdTipoRecurso);
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

            gestionRecursosServicio.obtenerListaEtapas(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        vm.Etapas = response.data;
                    });
                });
        }

        function obtenerListaTipoEntidad() {

            gestionRecursosServicio.obtenerListaTipoEntidad(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        var listaTiposEntidades = [];
                        var arreglolistaTipoEntidades = response.data;
                        for (var ls = 0; ls < arreglolistaTipoEntidades.length; ls++) {
                            if ((arreglolistaTipoEntidades[ls].Name == "Departamentos") || (arreglolistaTipoEntidades[ls].Name == "Municipios") ||
                                (arreglolistaTipoEntidades[ls].Name == "Privadas") || (arreglolistaTipoEntidades[ls].Name == "Entidades Presupuesto Nacional - PGN") ||
                                (arreglolistaTipoEntidades[ls].Name == "Minoría Étnica – Resguardo")) {
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
            gestionRecursosServicio.obtenerSectores(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        vm.listaSectores = response.data;
                    });
                });
        }

        function obtenerDepartamentos() {

            var listaDepartamentos = [];

            return gestionRecursosServicio.obtenerListaEntidades(parametros, 3)
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

            return gestionRecursosServicio.obtenerListaEntidades(parametros, 3)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaEntidadesComunidades = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidadesComunidades = arreglolistaEntidadesComunidades.filter(item => item.EntityTypeId == 10 && item.ParentId == null)
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

            var listaEntidades = [];
            return gestionRecursosServicio.obtenerListaEntidades(parametros, vm.model.Departamento.Id)
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
            return gestionRecursosServicio.obtenerListaEntidades(parametros, vm.model.Comunidad.Id)
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

            if (vm.proyectoId == null)
                if ($uibModalInstance.proyectoId != null) vm.proyectoId = $uibModalInstance.proyectoId;

            if (vm.nombrePrivadoRecurso == true) {
                var params = {
                    ProyectoId: vm.proyectoId,
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
                    ProyectoId: vm.proyectoId,
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

            gestionRecursosServicio.agregarFuentesFinanciacionProyecto(params, usuarioDNP, $uibModalInstance.idInstancia, $uibModalInstance.idAccion, $uibModalInstance.idNivel)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        //guardarCapituloModificado();
                        $sessionStorage.close();
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });

        }

        async function _listarTiposEntidades(tipoEntidad) {

            var listaEntidades = [];
            return gestionRecursosServicio.obtenerListaEntidades(parametros, tipoEntidad)
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
            //if ($uibModalInstance.InstanciaSeleccionada.IdEntidad != null)
            //    idEntidad = $uibModalInstance.InstanciaSeleccionada.IdEntidad;
            //var idEntidad = vm.model.Entidad.idEntidad;

            return gestionRecursosServicio.ObtenerListaTiposRecursosxEntidad(parametros, idEntidad)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;
                    var arregloRecursos = jQuery.parseJSON(respuesta.data);
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

        function guardarCapituloModificado() {
            var data = {
                ProyectoId: $uibModalInstance.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
                InstanciaId: $uibModalInstance.idInstancia,
                Modificado: false,
            }
            gestionRecursosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        //  vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        //vm.callBack();
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            //var FaseGuid = constantesBackbone.idEtapaNuevaEjecucion;
            //var Capitulo = 'Fuentes de financiación';
            //var Seccion = 'Recursos';

            //return gestionRecursosServicio.ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, $uibModalInstance.usuario.permisos.IdUsuarioDNP).then(
            //    function (respuesta) {
            //        if (respuesta.data != null && respuesta.data != "") {
            //            vm.seccionCapitulo = respuesta.data;
            //        }
            //    });
            const span = document.getElementById('id-capitulo-justificacionjustificacion');
            vm.seccionCapitulo = span.textContent;

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
    }

    angular.module('backbone').controller('modalAgregarFuenteController', modalAgregarFuenteController);

})();
