/// <reference path="../../model/UsuarioNotificacionConfigModel.js" />

(function () {
    'use strict';

    crearActualizarNotificacionController.$inject = [
        '$scope',
        '$routeParams',
        '$location',
        '$uibModal',
        '$filter',
        'UsuarioNotificacionConfigModel',
        'servicioNotificacionesMantenimiento',
        'servicioUsuarios',
        'archivoServicios',
        'utilidades',
        'array.extensions',
    ];

    function crearActualizarNotificacionController(
        $scope,
        $routeParams,
        $location,
        $uibModal,
        $filter,
        UsuarioNotificacionConfigModel,
        servicioNotificacionesMantenimiento,
        servicioUsuarios,
        archivoServicios,
        utilidades) {
        const vm = this;

        //#region Variables
        const model = new UsuarioNotificacionConfigModel();
        vm.idAplicacion = "configNotificaciones";
        vm.extension = "";

        const _validadores = [
            (model) => ([{ invalido: !model.NombreNotificacion, mensaje: "Ingrese un nombre para la notificación" }]),
            (model) => ([{ invalido: !model.Tipo, mensaje: "Seleccione un tipo de notificación" }]),
            (model) => ([{ invalido: !model.FechaInicio, mensaje: "Ingrese una fecha de inicio" }]),
            (model) => ([{ invalido: !model.FechaFin, mensaje: "Ingrese una feach de fin" }]),
            (model) => ([{ invalido: model.EsManual == null || model.EsManual == undefined, mensaje: "Seleccione el receptor de la notificación" }]),
            (model) => ([{ invalido: !validarTextoContenido(), mensaje: "Ingrese un contenido para la notificación" }]),
            (model) => ([{ invalido: moment(model.FechaInicio).isSame(moment(model.FechaFin)), mensaje: "Ingrese una fecha de Inicio diferente a la fecha de Fin" }]),
            (model) => ([{ invalido: moment(model.FechaFin).isBefore(moment(model.FechaInicio)), mensaje: "Ingrese una fecha de Fin mayor que la fecha de Inicio" }]),
            (model) => ([{ invalido: model.EsManual && !model.UsuarioNotificaciones.length, mensaje: "Seleccione un usuario mínimo para la notificación" }]),
            (model) => ([{ invalido: !model.EsManual && model.UsuarioNotificaciones.some(x => x), mensaje: "Cuando una notificación es programada no puede tener usuários seleccionados" }]),
            (model) => ([{ invalido: model.EsManual && model.ProcedimientoAlmacenadoId, mensaje: "Cuando una notificación es manual no puede tener un procedimiento almacenado" }]),
            (model) => ([{ invalido: !model.EsManual && !model.ProcedimientoAlmacenadoId, mensaje: "Seleccione un prodecimiento de almacendo para la notificación" }])
        ];

        const validarTextoContenido = () => {
            return vm.quill.getText().trim() && vm.quill.root.innerHTML
        }

        const plantillaAccionesUsuario = 'src/app/notificacionesMantenimiento/plantillas/plantillaAccionesUsuarioNotificacion.html';

        const columnDef = [
            {
                field: 'NombreUsuario',
                displayName: 'Usuarios',
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
                cellTemplate: plantillaAccionesUsuario,
                width: '10%'
            }
        ];

        const columnDefProcedimiento = [
            {
                field: 'NombreUsuario',
                displayName: 'Usuarios Preseleccionados',
                enableHiding: false,
                width: '90%',
            }
        ];

        vm.model = model;
        vm.usuarioSeleccionado = {};
        vm.procedimientos = [];
        vm.usuarios = [];
        vm.cargandoUsuarios = false;
        vm.filename = "";

        vm.gridOptions = {
            paginationPageSizes: [5, 10, 15, 25, 50, 100],
            paginationPageSize: 5,
            columnDefs: columnDef,
            data: []
        }

        vm.gridOptionsProcedimiento = {
            paginationPageSizes: [5, 10, 15, 25, 50, 100],
            paginationPageSize: 5,
            columnDefs: columnDefProcedimiento,
            data: []
        }

        //#endregion

        //#region Metodos

        function _validarModel(model) {
            try {
                const criticas = [];
                _validadores.forEach(validator => {
                    validator(model).forEach(resultado => {
                        if (resultado.invalido)
                            criticas.push(resultado.mensaje);
                    });
                })

                if (criticas.length)
                    _mostarToast(criticas);

                return !criticas.length;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar las configuraçiones da notificación");
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

        function _complementarDatos() {
            (vm.model.UsuarioNotificaciones || []).forEach(n => n.UsuarioConfigNotificacionId = vm.model.Id);
        }

        function volver() {
            history.back()
        }

        function _listarUsuarios() {
            return servicioUsuarios.obtenerUsuarios()
                .then(response => {
                    vm.usuarios = (response.data || []).map(x => ({
                        IdUsuarioDNP: x.IdUsuarioDnp,
                        NombreUsuario: x.Nombre,
                        Visible: false,
                        UsuarioYaLeyo: false
                    }));
                })
                .catch(error => {
                    toastr.error("Hubo un error al cargar los usuários por nombre");
                    console.log(error);
                })
        }

        function _listaProcedimientos() {
            return servicioNotificacionesMantenimiento.obtenerListaProcedimientosAlmacenados()
                .then(response => {
                    vm.procedimientos = response.data;
                  
                })
                .catch(error => {
                    toastr.error("Hubo un error al cargar los procedimientos disponibles");
                    console.log(error);
                });
        }

        async function _cargarNotificacion(Id) {
            return await servicioNotificacionesMantenimiento.obtenerListaNotificaciones(({ ConfigNotificacionIds: [Id] }))
                .then(async (respuesta) => {
                    vm.model = new UsuarioNotificacionConfigModel(respuesta.data[0]);
                    vm.gridOptions.data = vm.model.UsuarioNotificaciones;
                    vm.quill.root.innerHTML = vm.model.ContenidoNotificacion || "";
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar configuraciones de la notificación")
                });
        }

        function adjuntarArchivo() {
            document.getElementById('file').value = "";
            document.getElementById('file').click();
        }

        $scope.fileNameChanged = function (input) {
            vm.filename = input.files[0].name;
            cargarArchivo(input);
        }

        function _initializeQuill() {
            const containerEditor = document.getElementById('editor');
            if (!containerEditor)
                return;

            const toolbarOptions = [
                [{ 'font': [] }],
                [{ 'color': [] }, { 'background': [] }],
                ['bold', 'italic', 'underline', 'strike'],
                [{ 'header': 1 }, { 'header': 2 }],
                ['link', 'image'],
                ['blockquote', 'code-block'],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                [{ 'script': 'sub' }, { 'script': 'super' }],
                [{ 'indent': '-1' }, { 'indent': '+1' }],
                [{ 'direction': 'rtl' }],
                [{ 'align': [] }],
                [{ 'size': ['small', false, 'large', 'huge'] }],
                [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                ['clean']
            ];

            const optionsQuill = {
                placeholder: 'Ingrese algún contenido...',
                theme: 'snow',
                modules: {
                    toolbar: toolbarOptions
                }
            };

            vm.quill = new Quill(containerEditor, optionsQuill);
            vm.quill.root.innerHTML = "";

            vm.quill.on('text-change', function () {
                vm.model.ContenidoNotificacion = vm.quill.root.innerHTML;
            });
        }

        function preVisualizar() {
            if (!vm.quill.root.innerHTML) {
                toastr.warning("Ingrese algun contenido en texto de la notificación");
                return
            }

            if (!vm.model.Tipo) {
                toastr.warning("Seleccione un tipo de notificación");
                return
            };

            servicioNotificacionesMantenimiento.visualizarContenidoNotificacion(vm.model);
        }

        function guardar() {
            _complementarDatos();
            const valido = _validarModel(vm.model);
            if (!valido)
                return;

            servicioNotificacionesMantenimiento.guardarConfigNotificacion(vm.model)
                .then((respuesta) => {
                    const { data } = respuesta;
                    if (data.Id && data.Id > 0) {
                        toastr.success("Configuraciones de Notificación guardada con éxito");
                        $location.path("/NotificacionesMantenimiento/ListaNotificaciones");
                    }
                })
                .catch((error) => {
                    if (error.status == 400) {
                        const { Data } = error.data;
                        _mostarToast(Data);
                        return;
                    }

                    toastr.error("Error inesperado al guardar la notificación");
                });
        }

        function adicionarUsuario(usuario) {
            if (!usuario) {
                toastr.warning("Seleccione un usuário para agregar");
                return;
            }

            if (vm.model.UsuarioNotificaciones.some(x => x.IdUsuarioDNP == usuario.IdUsuarioDNP)) {
                toastr.warning("El usuário seleccionado ya se agregó");
                return;
            }

            vm.model.UsuarioNotificaciones.push(usuario);
            vm.gridOptions.data = vm.model.UsuarioNotificaciones || [];
            vm.usuarioSeleccionado = null;
        }

        function removerUsuario(index) {
            if (index == null || index == undefined)
                return;

            vm.model.UsuarioNotificaciones.removeAt(index);
            vm.gridOptions.data = vm.model.UsuarioNotificaciones;
            vm.usuarioSeleccionado = null;
            return;
        }

        function cambiarReceptor() {
            if (vm.model.EsManual) {
                vm.model.ProcedimientoAlmacenadoId = null;
                vm.gridOptionsProcedimiento.data = [];
                return;
            }

            vm.usuarioSeleccionado = null;
            vm.model.UsuarioNotificaciones = [];
            vm.gridOptions.data = [];
        }

        async function previsualizarUsuarios() {
            if (!vm.model.ProcedimientoAlmacenadoId) {
                toastr.warning("Seleccione un procedimiento para mirar los usuarios que seran incluidos en la notificación")
                return;
            }

            const { data } = await servicioNotificacionesMantenimiento.obtenerUsuariosPorProcedimientoId(vm.model.ProcedimientoAlmacenadoId)
                .catch(error => {
                    if (error.status == 400) {
                        _mostarToast(error.data.Data || [error.data.MensajeRetorno] || ["Hunbo un error al ejecutar el procedimiento seleccioando"]);
                        return;
                    }

                    toastr.error("Error inesperado al ejecutar el procedimiento seleccionado");
                });

            vm.gridOptionsProcedimiento.data = data;
        }

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'exe': case 'bin': case 'src': case 'vbs': return false;
                default: return true;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        function cargarArchivo(input) {
            vm.extension = obtenerExtension(vm.filename);
            let archivo = {
                FormFile: input.files[0],
                Nombre: vm.filename,
                Metadatos: {
                    extension: vm.extension
                }
            };

            if (!validarExtension(vm.extension)) {
                toastr.warning('Extensión no permitida');
                return;
            }
            archivoServicios.cargarArchivo(archivo, vm.idAplicacion).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    //Actualizar el idArchivoBlob en el pago de inflexibilidad
                    vm.model.IdArchivo = response[0].idArchivo;
                    utilidades.mensajeSuccess($filter('language')('ArchivoGuardado'), false, false, false);
                    //guardar();
                }
            }, error => {
                console.log(error);
            });
        };

        async function init() {
            _initializeQuill();

            const promises = [
                _listarUsuarios(),
                _listaProcedimientos()
            ];

            vm.notificacionConfigId = $routeParams.id;
            if (vm.notificacionConfigId)
                promises.push(_cargarNotificacion(vm.notificacionConfigId));

            await Promise.all(promises);
        }

        vm.init = init;
        vm.adicionarUsuario = adicionarUsuario;
        vm.removerUsuario = removerUsuario;
        vm.guardar = guardar;
        vm.volver = volver;
        vm.preVisualizar = preVisualizar;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.cambiarReceptor = cambiarReceptor;
        vm.previsualizarUsuarios = previsualizarUsuarios;

        //#endregion
    }

    angular.module('backbone')
        .controller('crearActualizarNotificacionController', crearActualizarNotificacionController);
})();