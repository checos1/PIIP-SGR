(function () {
    'use strict';

    crearEditarCentroAyudaItemController.$inject = [
        '$scope',
        '$sce',
        '$rootScope',
        'params',
        '$uibModalInstance',
        'servicioCentroAyuda',
        'AyudaTemaListaItemModel',
        'AyudaTipoConstante',
        'utilidades'
    ];

    function crearEditarCentroAyudaItemController(
        $scope,
        $sce,
        $rootScope,
        params,
        $uibModalInstance,
        servicioCentroAyuda,
        AyudaTemaListaItemModel,
        AyudaTipoConstante,
        utilidades
    ) {
        var vm = this;
        vm.AyudaTipoConstante = AyudaTipoConstante;
        vm.params = params;
        vm.model = {};

        vm.estados = [
            {
                descricion: 'Activo',
                valor: true
            },
            {
                descricion: 'Inactivo',
                valor: false
            }
        ];

        const _validadores = [
            (model) => ([{ invalido: !model.Nombre, mensaje: "Ingrese un nombre para la Ayuda" }]),
            (model) => ([{ invalido: !model.Descripcion, mensaje: "Ingrese una descripción para la Ayuda" }]),
            (model) => ([{ invalido: model.Activo == undefined || model.Activo == null, mensaje: "Seleccione un estado para la ayuda" }]),
            (model) => ([{ invalido: !validarContenidoRequerido(model), mensaje: "Ingrese un contenido para la ayuda" }]),
            (model) => ([{ invalido: !validarTextoContenidoVideo(model), mensaje: "Ingre um contenido válido para el video"}])
        ];

        const validarTextoContenidoVideo = (model) => {
            const regx = new RegExp("<\s*iframe[^>]*>(.*?)<\s*/\s*iframe>", "gi");
            return vm.params.TipoAyuda != vm.AyudaTipoConstante.AyudaVideo ||
                model.Contenido && regx.test(model.Contenido);
        }

        const validarContenidoRequerido = (model) => {
            return [vm.AyudaTipoConstante.AyudaEspecifico, vm.AyudaTipoConstante.AyudaVideo]
                .includes(vm.params.TipoAyuda) && !model.Contenido || true;
        }

        function _validarModel(model) {
            try {
                const mensajes = [];
                _validadores.forEach(validator => {
                    validator(model).forEach(resultado => {
                        if (resultado.invalido)
                            mensajes.push(resultado.mensaje);
                    });
                })

                if (mensajes.length)
                    _mostarToast(mensajes);

                return !mensajes.length;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar la Entidad");
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

        vm.guardar = function () {
            _complementarDatos();
            const valid = _validarModel(vm.model);
            if(!valid)
                return;
            
            servicioCentroAyuda.crearActualizarAyudaTema(vm.model)
                .then((respuesta) => {
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    $rootScope.$broadcast('OnChangeItemAyuda', {
                        TipoAyuda: vm.params.TipoAyuda,
                        ItemAyuda: respuesta.data
                    });
                    $uibModalInstance.close('cerrar');
                })
                .catch(error => {
                    if (error.status == 400) {
                        const { Data } = error.data;
                        _mostarToast(Data || ["Hubo un error en validación del ayuda"]);
                        return;
                    }
    
                    toastr.error("Hubo un error al guardar cambios");
                })
        };

        vm.init = function() {
            _initializeQuill();

            if(vm.params.Item)
                _cargarDatos(vm.params.Item);
        }

        vm.bindVideo = function() {
            if(validarTextoContenidoVideo(vm.model))
                return $sce.trustAsHtml(vm.model.Contenido);
        }

        function _cargarDatos(item){
            vm.model = new AyudaTemaListaItemModel(item);
            vm.quill.root.innerHTML = vm.model.Contenido;
        }

        function _complementarDatos() {
            vm.model.Contenido = vm.params.TipoAyuda != vm.AyudaTipoConstante.AyudaEspecifico && vm.model.Contenido 
                || vm.quill && vm.quill.getText().trim() && vm.quill.root.innerHTML;

            vm.model.TemaGeneralId = vm.params.TemaGeneralId || undefined;
            vm.model.AyudaTipoEnum = vm.params.TipoAyuda == vm.AyudaTipoConstante.AyudaVideo ? 2 : 1
        }

        vm.cerrar = function () {
            $uibModalInstance.dismiss('cerrar');
        }

        function _initializeQuill(){
            const containerEditor = document.getElementById('editor');
            if(!containerEditor)
                return;

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
                placeholder: 'Ingrese algún contenido...',
                theme: 'snow',
                modules: {
                    toolbar: toolbarOptions
                }
            };

            vm.quill = new Quill(containerEditor, optionsQuill);
            vm.quill.root.innerHTML = null;
        }
    }

    angular.module('backbone.usuarios').controller('crearEditarCentroAyudaItemController', crearEditarCentroAyudaItemController);
})();