/// <reference path="../../../model/MensajeMantenimientoModel.js" />

(function () {
    'use strict';

    crearActualizarMensajeController.$inject = [
        '$scope',
        '$routeParams',
        '$location',
        '$uibModal',
        'MensajeMantenimientoModel',
        'mensajeServicio',
        'sesionServicios', 
        'backboneServicios',
        'autorizacionServicios',
        'array.extensions'
    ];

    function crearActualizarMensajeController(
        $scope, 
        $routeParams,
        $location,
        $uibModal,
        MensajeMantenimientoModel, 
        mensajeServicio,
        sesionServicios,
        backboneServicios,
        autorizacionServicios) {
        const vm = this;

        //#region Variables
        const model = new MensajeMantenimientoModel();
        const entidades = [];
        const tiposMensajes = [];
        const estadosMensaje = [
            {
                descricion: 'Activo',
                valor: 1
            },
            {
                descricion: 'Inactivo',
                valor: 2
            }
        ];

        vm.disclaimer = { show: false };
        
        const _preVisualzadores = {
            disclaimer: function() {
                vm.disclaimer = {
                    template: vm.quill.root.innerHTML,
                    type: vm.model.EstiloTipoMensaje,
                    preVisualizacion: true,
                    show: true
                };
            },
            popUp: function () {
                mensajeServicio.preVisualizarModalMensaje({
                    MensajeTemplate: vm.quill.root.innerHTML,
                    EstiloTipoMensaje: vm.model.EstiloTipoMensaje,
                    RestringeAcesso: vm.model.RestringeAcesso
                })
            }
        }
    
        const _validadores = [
            (mensaje) => ([{ invalido: !mensaje.NombreMensaje, mensaje: "Ingrese un nombre para la mensaje" }]),
            (mensaje) => ([{ invalido: !mensaje.TipoEntidad, mensaje: "Seleccione un tipo de entidad para el mensaje" }]),
            (mensaje) => ([{ invalido: !mensaje.FechaCreacionInicio, mensaje: "Ingrese una fecha de inicio" }]),
            (mensaje) => ([{ invalido: !mensaje.FechaCreacionFin, mensaje: "Ingrese una feach de fin" }]),
            (mensaje) => ([{ invalido: !mensaje.EstadoMensaje, mensaje: "Seleccione el estado del mensaje" }]),
            (mensaje) => ([{ invalido: !mensaje.MensajeTemplate, mensaje: "Ingrese un mensaje de texto para la alerta" }]),
            (mensaje) => ([{ invalido: !mensaje.TipoMensaje, mensaje: "Seleccione un tipo de mensaje" }]),
            (mensaje) => ([{ invalido: moment(mensaje.FechaCreacionInicio).isSame(moment(mensaje.FechaCreacionFin)), mensaje: "Ingrese una fecha de Inicio diferente a la fecha de Fin" }]),
            (mensaje) => ([{ invalido: moment(mensaje.FechaCreacionFin).isBefore(moment(mensaje.FechaCreacionInicio)), mensaje: "Ingrese una fecha de Fin mayor que la fecha de Inicio" }]),
            (mensaje) => ([{ invalido: !(mensaje.Roles || []).length, mensaje: "Ingrese una role mínima para el mensaje" }]),
        ];

        const plantillaAccionesRoles = 'src/app/mensajesMantenimiento/plantillas/plantillaAccionesRoles.html';

        const columnDef = [
            {
                field: 'NombreRol',
                displayName: 'Roles',
                enableHiding: false,
                width: '90%',
            }, 
            {
                field: 'accion',
                displayName: 'Acción',
                headerCellClass: 'text-center',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: plantillaAccionesRoles,
                width: '10%'
            }
        ];

        vm.tiposEntidades = autorizacionServicios.obtenerTiposEntidad();
        vm.entidades = entidades;
        vm.tieneEntidades = false;
        vm.estadosMensaje = estadosMensaje;
        vm.tiposMensajes = tiposMensajes;
        vm.model = model;
        vm.roles = [];
        vm.roleSeleccionada = {};
        vm.entidadSeleccionada = {};
        vm.peticion = obtenerPeticion();

        vm.gridOptions = {
            paginationPageSizes: [5, 10, 15, 25, 50, 100],
            paginationPageSize: 5,
            columnDefs: columnDef,
            data: vm.model.Roles
        }

        const containerEditor = document.getElementById('editor');
        const toolbarOptions = [
            [{ 'font': [] }],
            [{ 'color': [] }, { 'background': [] }],
            ['bold', 'italic', 'underline', 'strike'],
            [{ 'header': 1 }, { 'header': 2 }],
            ['link', 'image'],
            ['blockquote', 'code-block'],
            [{ 'list': 'ordered'}, { 'list': 'bullet' }],
            [{ 'script': 'sub'}, { 'script': 'super' }],
            [{ 'indent': '-1'}, { 'indent': '+1' }],
            [{ 'direction': 'rtl' }],
            [{ 'align': [] }],
            [{ 'size': ['small', false, 'large', 'huge'] }],
            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
            ['clean']
          ];

        const optionsQuill = {
            placeholder: 'Redactar texto...',
            theme: 'snow',
            modules: {
                toolbar: toolbarOptions
            }
         };

        vm.quill = new Quill(containerEditor, optionsQuill);
        
        //#endregion

        //#region Metodos

        function obtenerPeticion() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                return {
                    IdUsuarioDNP: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdObjeto: idTipoProyecto,
                    ListaIdsRoles: roles,
                  };
            }
        }

        function cambiarTipoEntidad() {
            if(!vm.model.TipoEntidad)
                return;

            vm.model.Entidades = [];
            if(vm.model.TipoEntidad == 'Nacional') {
                vm.tieneEntidades = false;
                vm.entidades = [];
                vm.model.Entidades = [];
                return;
            }
            
            _listarTiposEntidades(vm.model.TipoEntidad);
            vm.tieneEntidades = true;
        }

        function adicionarRole(role) {
            if(!role) {
                toastr.warning("Seleccione una role para agregar");
                return;
            }

            if(vm.model.Roles.some(x => x.Id == role.Id)) {
                toastr.warning("La role seleccionada ya se agregó");
                return;
            }

            vm.model.Roles.push(role);
            vm.roleSeleccionada = null;
        }

        function removerRole(index) {
            if (index == null || index == undefined)
                return;

            vm.model.Roles.removeAt(index);
            return;
        }

        function _validarMensaje(model) {
            try {
                const mensanjesCritica = [];
                _validadores.forEach(validator => {
                    validator(model).forEach(resultado => {
                        if (resultado.invalido)
                            mensanjesCritica.push(resultado.mensaje);
                    });
                })

                if (mensanjesCritica.length)
                    _mostarToast(mensanjesCritica);

                return !mensanjesCritica.length;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar la alerta");
                return false;
            }
        }

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        function guardar() {
            _complementarDatos();
            const valido = _validarMensaje(vm.model);
            if (!valido)
                return;

            mensajeServicio.guardarMensaje(vm.peticion, vm.model).then(exito, error);

            function exito(respuesta) {
                const { data } = respuesta;
                if(data.Id && data.Id > 0) {
                    toastr.success("Mensaje guardada con éxito");
                    $location.path("/listaMensajeMantenimiento");
                }
            }

            function error(respuesta) {
                if (respuesta.status == 400) {
                    const { Data } = respuesta.data;
                    _mostarToast(Data);
                    return;
                }

                toastr.error("Error inesperado al guardar mensaje");
            }
        }

        function _complementarDatos() {
            vm.model.MensajeTemplate = vm.quill.root.innerHTML;
            (vm.model.Entidades || []).forEach(entidad => entidad.IdMensaje = vm.model.Id);
            (vm.model.Roles || []).forEach(role => role.IdMensaje = vm.model.Id);
        }

        function volver() {
            history.back()
        }

        async function _listarTiposEntidades(tipoEntidad) {
            return mensajeServicio.obtenerListaTiposEntidades(tipoEntidad)
                .then(respuesta => {
                    if(!respuesta.data)
                        return;

                    vm.entidades = (respuesta.data || []).map(x => ({ 
                        Id: x.IdEntidad, 
                        NombreEntidad: x.NombreCompleto
                    }));

                    vm.tieneEntidades = vm.entidades.length > 0;
                 })
                 .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                 });
        }

        //TODO: Validar si obtener roles está correcto
        async function _listarRolesUsuario() {
            return autorizacionServicios.obtenerRolesPorUsuario(usuarioDNP)
                .then(roles => {
                    vm.roles = (roles || []).map(x => ({
                        Id: x.IdRol,
                        NombreRol: x.Nombre                        
                    }));
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las roles");
                })
        }

        function preVisualizarMensaje() {
            if(!vm.quill.root.innerHTML) {
                toastr.warning("Ingrese un mensaje de texto para la alerta");
                return
            }

            if(!vm.model.TipoMensaje) {
                toastr.warning("Seleccione un tipo de mensaje");
                return
            };

            const tipoMensaje = {
                1: "popUp",
                2: "disclaimer"
            }[vm.model.TipoMensaje];

            _preVisualzadores[tipoMensaje]();
        }

        async function _cargarMensaje(idMensaje) {
            return await mensajeServicio.obtenerMensajePorId(vm.peticion, idMensaje)
                .then(async (respuesta) => {
                    vm.model = new MensajeMantenimientoModel(respuesta.data[0]);
                    vm.gridOptions.data = vm.model.Roles;
                    vm.quill.root.innerHTML = vm.model.MensajeTemplate;

                    if(vm.model.TipoEntidad == "Nacional") {
                        vm.entidades = [];
                        vm.model.Entidades = [];
                        vm.tieneEntidades = false;
                        return
                    }
                    await _listarTiposEntidades(vm.model.TipoEntidad);
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar mensaje")
                });
        }

        async function init() {
            const promises = [ _listarRolesUsuario() ];
            await Promise.all(promises);

            vm.idMensaje = $routeParams.id;
            if (vm.idMensaje) {
                await _cargarMensaje(vm.idMensaje);
                return;
            }
        }

        vm.init = init;
        vm.cambiarTipoEntidad = cambiarTipoEntidad;
        vm.adicionarRole = adicionarRole;
        vm.removerRole = removerRole;
        vm.guardar = guardar;
        vm.volver = volver;
        vm.preVisualizarMensaje = preVisualizarMensaje;
        
        //#endregion
    }

    angular.module('backbone')
        .controller('crearActualizarMensajeController', crearActualizarMensajeController);
})();