(function () {
    centroAyudaMenuController.$inject = [
        '$scope',
        '$uibModal',
        'AyudaTemaListaItemModel',
        'AyudaTipoConstante',
        'array.extensions',
        'utilidades'
    ];

    function centroAyudaMenuController(
        $scope,
        $uibModal,
        AyudaTemaListaItemModel,
        AyudaTipoConstante,
        utilidades
    ) {
        const vm = this;

        vm.abaArticulo = true;
        vm.articulos = [];
        vm.videos = [];
        vm.listaOrigemArticulos = [];
        vm.listaOrigemVideos = [];
        vm.filtro = "";
        vm.filtrarAyuda = filtrarAyuda;
        vm.obtenerListaTemas = obtenerListaTemas;

        const _cambiosAyudas = {
            [AyudaTipoConstante.AyudaGeneral]: _cambioAyudaTemaGeneral,
            [AyudaTipoConstante.AyudaEspecifico]: _cambioAyudaSubTema,
            [AyudaTipoConstante.AyudaVideo]: _cambioAyudaVideo
        }

        const _eliminarAyudas = {
            [AyudaTipoConstante.AyudaGeneral]: (item) => {
                const itemEliminado = vm.articulos.find(x => x.Id == item.Id);
                vm.articulos.remove(itemEliminado);
            },
            [AyudaTipoConstante.AyudaEspecifico]: (item) => {
                const temaGeneral = vm.articulos.find(x => x.Id == item.TemaGeneralId);
                const subTema = temaGeneral.SubItems.find(x => x.Id == item.Id);
                temaGeneral.SubItems.remove(subTema);
            },
            [AyudaTipoConstante.AyudaVideo]: (item) => {
                const videoEliminado = vm.videos.find(x => x.Id == item.Id);
                vm.videos.remove(videoEliminado);
            }
        }

        function _cambioAyudaTemaGeneral(ayudaGeneral) {
            const temaGeneralExistente = vm.articulos.find(x => x.Id == ayudaGeneral.Id);

            if(temaGeneralExistente && !ayudaGeneral.Activo)
                return vm.articulos.remove(temaGeneralExistente);

            if(temaGeneralExistente && ayudaGeneral.Activo) {
                ayudaGeneral.SubItems = temaGeneralExistente.SubItems;
                vm.articulos.remove(temaGeneralExistente);
                vm.articulos.push(ayudaGeneral);
                return
            }

            if(ayudaGeneral.Activo)
                vm.articulos.push(ayudaGeneral);
        }

        function _cambioAyudaSubTema(ayudaSubTema) {
            const temaGeneral = vm.articulos.find(x => x.Id == ayudaSubTema.TemaGeneralId);
            const temaExistente = (temaGeneral.SubItems || []).find(x => x.Id == ayudaSubTema.Id);
            
            if(temaExistente && !ayudaSubTema.Activo)
                return temaGeneral.SubItems.remove(temaExistente);
                
            if(temaExistente && ayudaSubTema.Activo)
                return _actualizaItemAyuda(temaGeneral.SubItems, ayudaSubTema, temaExistente);

            if(ayudaSubTema.Activo)
                temaGeneral.SubItems.push(ayudaSubTema);
        }

        function _cambioAyudaVideo(ayudaVideo) {
            const ayudaVideoExistente = vm.videos.find(x => x.Id == ayudaVideo.Id);
            
            if(ayudaVideoExistente && !ayudaVideo.Activo)
                return vm.videos.remove(ayudaVideoExistente);

            if(ayudaVideoExistente && ayudaVideo.Activo)
                return _actualizaItemAyuda(vm.videos, ayudaVideo, ayudaVideoExistente);

            if(ayudaVideo.Activo)
                vm.videos.push(ayudaVideo)
        }

        function _actualizaItemAyuda(arr, ayudaNueva, ayudaAntiguo) {
            arr.remove(ayudaAntiguo);
            arr.push(ayudaNueva);
        }

        vm.$onInit = function () {};

        function obtenerListaTemas(data) {
            const temas = (data || []).map(x => new AyudaTemaListaItemModel(x));
            vm.articulos = temas.filter(x => x.AyudaTipoEnum == 1);
            vm.listaOrigemArticulos = vm.articulos;
            vm.videos = temas.filter(x => x.AyudaTipoEnum == 2);
            vm.listaOrigemVideos = vm.videos;
        }

        function filtrarAyuda() {
            if (vm.abaArticulo) {
                vm.articulos = vm.listaOrigemArticulos;
                if (vm.filtro == "")
                    return;

                var filtro = toNormalForm(vm.filtro.toUpperCase());

                vm.articulos = vm.articulos.filter(x => toNormalForm(x.Nombre.toUpperCase()).includes(filtro));

                let subItens = [];
                vm.listaOrigemArticulos.forEach(item => {
                    let filter = item.SubItems.filter(x => toNormalForm(x.Nombre).toUpperCase().includes(filtro));
                    if (filter != undefined) {
                        filter.forEach(it => {
                            subItens.push(it);
                        });
                    }
                });

                if (subItens.length > 0) {
                    vm.articulos.push({
                        Activo: true,
                        AyudaTipoEnum: 1,
                        Contenido: "Resultados",
                        Descripcion: "Resultados",
                        Id: "Resultado",
                        Nombre: "Resultados específicos",
                        SubItems: subItens
                    })
                }
            } else {
                vm.videos = vm.listaOrigemVideos;
                var filtro = toNormalForm(vm.filtro.toUpperCase());
                vm.videos = vm.videos.filter(x => toNormalForm(x.Nombre).toUpperCase().includes(filtro));
            }
        }

        function toNormalForm(str) {
            return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
        }
        
        vm.visualizarArticulo = function (subItem) {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/centroAyuda/modales/exhibicionContenidoAyudaItem/exhibicionContenidoAyudaItem.html',
                controller: 'exhibicionContenidoAyudaItemController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "modal__ayuda__exhibicion",
                resolve: {
                    params: {
                        AyudaTema: subItem
                    }                    
                },
            });
        }

        vm.cambioAba = function (aba) {
            vm.articulos = vm.listaOrigemArticulos;
            vm.videos = vm.listaOrigemVideos;
            vm.abaArticulo = aba == 'articulos';
        }

        vm.showMore = function(text) {
            const textSinEspaso = text.replace(/ /g,'');
            return text.split(" ").length > 1 && textSinEspaso.length >= 64 
                || text.split(" ").length == 1 && text.length >= 39
        }

        vm.toggleShow = function(video) {
            video.showMore = !video.showMore; 
        }

        $scope.$on('OnChangeItemAyuda', function (event, data) {
            _cambiosAyudas[data.TipoAyuda](data.ItemAyuda);
        });

        $scope.$on('OnDeleteItemAyuda', function (event, data) {
            _eliminarAyudas[data.TipoAyuda](data.ItemAyuda);
        });

        $scope.$on('ListaTemasBroadcast', function (event, data) { 
            vm.obtenerListaTemas(data);
        });
    }

    angular.module("backbone").component("centroAyuda", {
        templateUrl: "src/app/centroAyuda/centroAyudaMenu/centroAyudaTemplate.html",
        controller: centroAyudaMenuController,
        controllerAs: "vm"
    });
})();