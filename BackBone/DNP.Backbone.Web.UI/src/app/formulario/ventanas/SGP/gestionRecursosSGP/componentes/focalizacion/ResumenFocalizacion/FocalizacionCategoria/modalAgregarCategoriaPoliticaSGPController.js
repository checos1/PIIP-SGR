(function () {
    'use strict';

    angular.module('backbone')
        .controller('modalAgregarCategoriaPoliticaSgpController', modalAgregarCategoriaPoliticaSgpController);

    modalAgregarCategoriaPoliticaSgpController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$sessionStorage',
        'utilidades',
        'focalizacionAjustesSGPServicio',
        '$filter',
        'categoria'
    ];

    function modalAgregarCategoriaPoliticaSgpController(
        $scope,
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        focalizacionAjustesSGPServicio,
        $filter,
        categoria
    ) {
        const vm = this;

        //#region Variables
        vm.options = [];
        vm.categoria = categoria;
        vm.agregarOtro = false;
        vm.textoEntregable = "";
        vm.mostrarError = false;
        vm.mostrarErrorLongitud = false;
        vm.btnGuardarDisable = true;
        vm.nombrePolitica = "";
        //#endregion

        //#region Metodos

        function consultarCategoriasPolitica(idPadre, nombrePolitica) {
            var esGrupoEtnico = 0;
            vm.nombrePolitica = nombrePolitica.toUpperCase();
            esGrupoEtnico = nombrePolitica.includes('GRUPOS ÉTNICOS') ? 1 : 0 || nombrePolitica.includes('Grupos étnicos') ? 1 : 0;
            var idEntidad = $sessionStorage.InstanciaSeleccionada.IdEntidad;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
            var categorias = $sessionStorage.categoriasXPolitica;

            vm.politica = idPadre;
            return focalizacionAjustesSGPServicio.ConsultarCategoriasSubcategoriasSGP(idPadre, idEntidad, 1, esGrupoEtnico, usuarioDNP).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        var Categorias = JSON.parse(respuesta.data);
                        var arregloCategorias = JSON.parse(Categorias);

                        var listaCategoriaPolitica = [];

                        for (var pl = 0; pl < arregloCategorias.length; pl++) {

                            var categoriaPolitica = {
                                categoriaId: arregloCategorias[pl].Id,
                                categoriaNombre: arregloCategorias[pl].Name,
                                categoriaPadre: arregloCategorias[pl].ParentId,
                                categoriaNivel: arregloCategorias[pl].PolicyTargetingTypeId,
                                deshabilitar: true
                            }

                            if (arregloCategorias.indexOf(arregloCategorias[pl].Id) === -1) {
                                listaCategoriaPolitica.push(categoriaPolitica);
                            }
                        }

                        vm.listaCategoriaPoliticas = angular.copy(listaCategoriaPolitica);
                    }
                });
        }

        function cambiarCategoria(tipo) {
            if (tipo === 1) {
                if (!vm.model.idCategoria)
                    return;
                consultarSubcategorias(vm.model.idCategoria);
            } else {
                if (!vm.model.idSubcategoria)
                    return;
                vm.btnGuardarDisable = false;
            }
        }

        function consultarSubcategorias(idCategoria) {

            var esGrupoEtnico = 0;
            var nombrePilitica = vm.nombrePolitica.toUpperCase();
            esGrupoEtnico = vm.nombrePolitica.includes('GRUPOS ÉTNICOS') ? 1 : 0 || vm.nombrePolitica.includes('Grupos étnicos') ? 1 : 0;;

            var idEntidad = $sessionStorage.InstanciaSeleccionada.IdEntidad;
            var subCategorias = $sessionStorage.categoriasXPolitica;
            return focalizacionAjustesSGPServicio.ConsultarCategoriasSubcategoriasSGP(idCategoria, idEntidad, 0, esGrupoEtnico, usuarioDNP).then(
                //return focalizacionAjustesServicio.ConsultarPoliticasCategoriasPorPadre(idCategoria, usuarioDNP).then(
                function (respuesta) {
                    //var resp = respuesta.data;
                    var Subcategorias = JSON.parse(respuesta.data);
                    var arregloSubcategorias = JSON.parse(Subcategorias);
                    vm.btnGuardarDisable = false;
                    //if (resp.length > 2) {
                    //if (arregloSubcategorias.length > 2) {
                    //if (respuesta.data != null && respuesta.data != "") {
                    if (arregloSubcategorias != null && arregloSubcategorias != "") {
                        vm.tieneSubcategoria = true;

                        var listaSubcategoriaPolitica = [];

                        for (var pl = 0; pl < arregloSubcategorias.length; pl++) {

                            var subcategoriaCategoria = {
                                categoriaId: arregloSubcategorias[pl].Id,
                                categoriaNombre: arregloSubcategorias[pl].Name,
                                categoriaPadre: arregloSubcategorias[pl].ParentId,
                                categoriaNivel: arregloSubcategorias[pl].PolicyTargetingTypeId,
                                deshabilitar: true
                            }

                            if (arregloSubcategorias.indexOf(arregloSubcategorias[pl].Id) === -1) {
                                listaSubcategoriaPolitica.push(subcategoriaCategoria);
                            }
                        }

                        vm.listaSubcategoriaPoliticas = angular.copy(listaSubcategoriaPolitica);
                        vm.btnGuardarDisable = true;
                        //}
                    } else {
                        vm.tieneSubcategoria = false;
                    }
                });
        }

        function guardar() {
            let lstCategorias = $sessionStorage.categoriasXPolitica;

            let tieneSubcategoria = vm.tieneSubcategoria;
            //let listaDimension = [];
            let nombrePolitica = vm.categoria.nombrePoliticaCat;
            let idPolitica = vm.categoria.idpolitica;
            let categoriaId = tieneSubcategoria ? vm.model.idSubcategoria : vm.model.idCategoria;

            let existsCategoria = lstCategorias.find(x => x == categoriaId);
            if (existsCategoria) {
                utilidades.mensajeError("La categoría o subcategoría seleccionada ya se encuentra registrada. Por favor validar");
                return;
            }

            //if (tieneSubcategoria) {
            //    listaDimension = vm.listaSubcategoriaPoliticas;
            //    var subCat = vm.model.idSubcategoria;
            //} else {
            //    listaDimension = vm.listaCategoriaPoliticas;
            //    var cat = vm.model.idCategoria;
            //}

            var parametros = {
                ProyectoId: $sessionStorage.idProyectoEncabezado,
                PoliticaId: idPolitica,
                CategoriaId: categoriaId
                //CategoriaId: subCat == null ? cat : subCat
            };

            return focalizacionAjustesSGPServicio.guardarFocalizacionCategoriasPoliticaSGP(parametros, usuarioDNP)
                .then(function (response) {
                    let res = JSON.parse(response.data);
                    if (res) {
                        if (res.Exito === true) {
                            vm.options.push(parametros);
                            utilidades.mensajeSuccess('Usted puede visualizarla en la parte inferior del listado de categorías de la política "' + nombrePolitica + '".', false, false, false, 'La categoría ha sido agregada y guardada con éxito.');
                            //$sessionStorage.close(vm.options);
                            $uibModalInstance.close(vm.options);
                        }
                        else {
                            var mensaje = response.data.Mensaje;
                            utilidades.mensajeError(mensaje.substr(mensaje.indexOf(':') + 1), false);
                        }
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
        }

        function cerrar() {
            vm.options = [];
            $uibModalInstance.close();
        }

        function toggleGuardar() {
            if ((vm.options && vm.options.length > 0) || vm.agregarOtro) {
                vm.btnGuardarDisable = false;
            }
            else {
                vm.btnGuardarDisable = true;
            }
        };

        function toggleCurrency(entregable) {
            if (entregable.isChecked) {

                let entregableAgreagar = new Object();
                entregableAgreagar.nombre = entregable.EntregableNombre;
                entregableAgreagar.etapa = vm.Producto.Etapa;
                entregableAgreagar.productoId = vm.Producto.ProductoId;
                entregableAgreagar.deliverable = vm.Producto.AplicaEDT;
                entregableAgreagar.deliverableCatalogId = entregable.EntregableId;

                vm.options.push(entregableAgreagar);

            } else {
                var toDel = vm.options.map(function (e) { return e.deliverableCatalogId; }).indexOf(entregable.EntregableId);
                vm.options.splice(toDel, 1);
            }

            toggleGuardar();
        };



        function seleccionarNuevo(nuevo) {
            vm.agregarOtro = nuevo.isChecked;
            toggleGuardar();
        };

        function init() {            
            consultarCategoriasPolitica(vm.categoria.idpolitica, vm.categoria.nombrePoliticaCat);
        }

        //métodos
        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.toggleCurrency = toggleCurrency;
        vm.seleccionarNuevo = seleccionarNuevo;
        vm.consultarCategoriasPolitica = consultarCategoriasPolitica;
        vm.cambiarCategoria = cambiarCategoria;
        vm.tieneSubcategoria = false;

        //listas
        vm.listaCategoriaPoliticas = [];
        vm.listaSubcategoriaPoliticas = [];
        vm.options = [];
        //#endregion
    }
})();