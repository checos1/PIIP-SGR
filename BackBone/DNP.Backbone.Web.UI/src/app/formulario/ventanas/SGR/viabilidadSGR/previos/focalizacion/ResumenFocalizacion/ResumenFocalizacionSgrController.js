(function () {
    'use strict';

    ResumenFocalizacionSgrController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
        '$timeout', 'focalizacionAjustesSgrServicio', 'justificacionCambiosServicio'
    ];

    function ResumenFocalizacionSgrController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        $timeout,
        focalizacionAjustesSgrServicio,
        justificacionCambiosServicio
    ) {
        var listaFuentesBase = [];

        var vm = this;
        vm.init = init;
        vm.listaPoliticasProyecto = [];
        vm.listaPoliticasCategorias = null;
        vm.listaPoliticasCategoriasIndicadores = null;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.habilitaVerPoliticasIndicadores = 0;
        vm.lblIndicadoresODSPND = "";
        vm.lblIndicadoresPMI = "";
        vm.abrirModalAgregarIndicador = abrirModalAgregarIndicador;
        vm.categoriaSeleccionada = "";
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.habilitaAgregarCategorias = false;
        vm.habilitaEliminarCategorias = false;
        //vm.eliminarPolitica = eliminarPolitica;
        vm.flujoaprobacion = "";
        vm.permiteEditar = true;
        vm.erroresComponente = [];
        /* ------------ Estructura necesaria para botón validar --------------- */

        vm.nombreComponente = "sgrviabilidadpreviosfocalizacioncategoriapoliticas";
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.componentesRefresh = [
            'sgrviabilidadpreviosfocalizacionpoliticas',
            'sgrviabilidadpreviosfocalizacioncrucepoliticas',
            'sgrviabilidadpreviosfocalizacioncategoriapoliticas',
            'sgrviabilidadpreviosrecursosfuentessgr',
            'sgrviabilidadpreviosrecursosfuentesnosgr'
        ];
        vm.componentesRefreshEliminar = [
            'sgrviabilidadpreviosrecursosfuentessgr',
            'sgrviabilidadpreviosrecursosfuentesnosgr'
        ];

        vm.currentYear = new Date().getFullYear();

        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.seccionCapitulo = null;

        /*Carga Masiva*/
        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.GuardarArchivoFocalizacion = GuardarArchivoFocalizacion;
        vm.exportarFocalizacionExcel = exportarFocalizacionExcel;
        vm.abrirModalAgregarCategoriaPolitica = abrirModalAgregarCategoriaPolitica;
        vm.eliminarCategoriaPolitica = eliminarCategoriaPolitica;
        vm.nombrearchivo = "";
        vm.Focalizacion = null;
        vm.FocalizacionArchivo = [];

        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
        vm.ConvertirNumero4decimales = ConvertirNumero4decimales;
        vm.abrirMensajeInformacionRegionalizacion = abrirMensajeInformacionRegionalizacion;
        vm.abrirMensajeMetaCategoria = abrirMensajeMetaCategoria;
        vm.abrirMensajeMetaIndicador = abrirMensajeMetaIndicador;
        vm.abrirMensajeArchivoFocalizacion = abrirMensajeArchivoFocalizacion;
        vm.valoresEnCero = valoresEnCero;
        vm.obtenerIndicador = obtenerIndicador;
        vm.obtenerIndicadorAcumulable = obtenerIndicadorAcumulable;
        vm.obtenerUnidadMedidaIndicadorSecundario = obtenerUnidadMedidaIndicadorSecundario;
        vm.obtenerMetaIndicadorSecundario = obtenerMetaIndicadorSecundario;
        vm.obtenerAcumulativoIndicadorSecundario = obtenerAcumulativoIndicadorSecundario;
        vm.longMaxText = 30;
        vm.BuscarValoresCategoria = BuscarValoresCategoria;

        vm.HabilitarSubirArchivo = true;
        vm.IndicePoliticaEtiquetaError = 0;
        vm.IndiceCategoriaEtiquetaError = 0;

        vm.disabled = $sessionStorage.soloLectura;
        vm.activar = true;

        function abrirMensajeArchivoFocalizacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Plnatilla Carga Masiva Focalizacion Categorias, </span><br /> <span class='tituhori'><ul><li>No se permite texto en la columna de 'En ajuste $' y 'Meta categoría', 'Personas categoría', 'Meta indicador secundario'</li><li>La columna 'Meta categoría' acepta valores numéricos sin separador de mil y cuatro decimales con separador coma(,)</li><li>La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li></ul></span>");
        }

        vm.AbrilNivel1 = function (fuenteId, indexPoliticas, indexCategorias) {
            var variable = $("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias);
            var imgmenos = document.getElementById("imgmenos-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias);
            if (variable === "+") {
                $("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }
        vm.AbrilNivel2 = function (fuenteId, productoId, indexPoliticas, indexCategorias) {
            var variable = $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias);
            var imgmenos = document.getElementById("imgmenos" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias);
            if (variable === "+") {
                $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }
        vm.AbrilNivel3 = function (productoId, fuenteId, localizacionId, indexPoliticas, indexCategorias) {
            var variable = $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
            var imgmenos = document.getElementById("imgmenos-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
            var detail = $("#detail-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
            if (variable === "+") {
                $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                if (detail != undefined) {

                    detail.removeClass("hidden");
                    detail[0].classList.remove("hidden");
                    $("#detail-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).removeClass("hidden");
                }
            } else {
                $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                if (detail != undefined) detail[0].classList.add("hidden");
            }
        }
        vm.existeFocalizacion = false;

        vm.volver = function () {
            $(window).scrollTop($('#fuentes').position().top + 500);
        }
        vm.VerPolitica = function (idPolitica) {
            document.getElementById("politica-" + idPolitica).style.display = "block";
            document.getElementById("politica-" + idPolitica + "-titulo").style.display = "block";
            if ($("#politica-" + idPolitica) != undefined && $("#politica-" + idPolitica).position() != undefined) $(window).scrollTop($("#politica-" + idPolitica).position().top + 250);
        }
        vm.TieneCategorias = function (itemPoliticas) {
            let tiene = itemPoliticas.Categorias != null && itemPoliticas.Categorias.length > 0;
            return tiene;
        }
        vm.VerPoliticaIndicadores = function (PoliticaId) {
            document.getElementById("politicaI-" + PoliticaId).style.display = "block";
            if ($("#politicaI-" + PoliticaId) != undefined && $("#politicaI-" + PoliticaId).position() != undefined) $(window).scrollTop($("#politicaI-" + PoliticaId).position().top + 200);
        }

        vm.AbrilNivelIndicadores = function (PoliticaId, Categoria1Id, Categoria2Id) {
            var variable = $("#ico2-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id)[0].innerText;
            var imgmas = document.getElementById("imgmas-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id);
            var imgmenos = document.getElementById("imgmenos-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id);
            if (variable === "+") {
                $("#ico2-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico2-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.mostrarOcultar = function (indexPoliticas, indexCategorias, indexFuentes, indexProducto) {

            $("#div-producto-mas-" + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto).toggleClass('hidden');
            $("#div-producto-menos-" + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto).toggleClass('hidden');
        }

        vm.mostrarOcultarSubCategoria = function (indexPoliticas, indexCategorias) {

            $("#div-subcategoria-mas-" + indexPoliticas + "-" + indexCategorias).toggleClass('hidden');
            $("#div-subcategoria-menos-" + indexPoliticas + "-" + indexCategorias).toggleClass('hidden');
        }

        function init() {
            
           

            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });

        }

        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente + '-1') {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                vm.model = {
                    modulos: {
                        administracion: false,
                        backbone: true
                    }
                }
                vm.FocalizacionArchivo = [];
                vm.obtenerPoliticasTransversales(vm.BPIN);

                vm.obtenerPoliticasTransversalesCategorias(vm.BPIN);
                vm.ConsultarPoliticasCategoriasIndicadores(vm.BPIN);

                if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                    $timeout(function () {
                        vm.notificacionValidacionPadre(vm.erroresComponente);
                    }, 600);
                }

                vm.flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

                if (vm.flujoaprobacion === $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                    vm.permiteEditar = false;
                    bloquearControles();
                }

                if ($scope.$parent !== undefined && $scope.$parent.$parent !== undefined && $scope.$parent.$parent.vm !== undefined && $scope.$parent.$parent.vm.HabilitarGuardarPaso !== undefined && !$scope.$parent.$parent.vm.HabilitarGuardarPaso) {
                    bloquearControles();
                }
            }
        }

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        }); 

        function bloquearControles() {
            vm.disabled = true;
            vm.HabilitarSubirArchivo = false;
            $("#btnCargarArchivo").attr('disabled', true);
        }

        vm.refrescarResumenCostos = function () {
        }

        vm.obtenerPoliticasTransversales = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;
            var idInstanciaProyecto = $sessionStorage.idInstancia;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;

            //consultarDatosCofinanciador();

            return focalizacionAjustesSgrServicio.obtenerPoliticasTransversalesProyecto(idInstanciaProyecto, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        //var cadena = '{"ProyectoId":null,"BPIN":"202200000000064","Politicas":[{"PoliticaId":1,"Politica":"ACTIVIDADES DE CIENCIA, TECNOLOGÍA E INNOVACIÓN","EnProyecto":true,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":2,"Politica":"CAMBIO CLIMÁTICO","EnProyecto":true,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":3,"Politica":"CAMPESINOS","EnProyecto":false,"EnSeguimiento":true,"EnFirme":false},{"PoliticaId":4,"Politica":"CONSTRUCCIÓN DE PAZ","EnProyecto":false,"EnSeguimiento":true,"EnFirme":false},{"PoliticaId":5,"Politica":"DESPLAZADOS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":6,"Politica":"DISCAPACIDAD E INCLUSIÓN SOCIAL","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":7,"Politica":"EQUIDAD DE LA MUJER","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":8,"Politica":"GESTIÓN DE RIESGO DE DESASTRES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":9,"Politica":"GRUPOS ÉTNICOS - COMUNIDADES RROM","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":10,"Politica":"GRUPOS ÉTNICOS - INDIGENAS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":11,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN AFROCOLOMBIANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":12,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN PALENQUERA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":13,"Politica":"GRUPOS ÉTNICOS - POBLACIÓN RAIZAL","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":14,"Politica":"PARTICIPACIÓN CIUDADANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":15,"Politica":"PLAN NACIONAL DE CONSOLIDACIÓN","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":16,"Politica":"RED UNIDOS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":17,"Politica":"SEGURIDAD ALIMENTARIA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":18,"Politica":"SEGURIDAD Y CONVIVENCIA CIUDADANA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":19,"Politica":"TECNOLOGÍAS DE INFORMACIÓN Y COMUNICACIONES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":20,"Politica":"VÍCTIMAS","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1506,"Politica":"POLITICA DE PRUEBA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1507,"Politica":"CONTROL A LA DEFORESTACIÓN Y GESTIÓN DE LOS BOSQUES","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1508,"Politica":"PRIMERA INFANCIA, INFANCIA Y ADOLESCENCIA","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false},{"PoliticaId":1509,"Politica":"ZONAS FUTURO","EnProyecto":false,"EnSeguimiento":false,"EnFirme":false}]}';
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        //var arregloGeneral = jQuery.parseJSON(cadena)
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosGenerales = arregloGeneral.Politicas;

                        var listaPoliticasProy = [];

                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            var habilitarVerDatos = false;
                            if (arregloDatosGenerales[pl].Politica === 'CONSTRUCCIÓN DE PAZ' || arregloDatosGenerales[pl].Politica === 'EQUIDAD DE LA MUJER') {
                                habilitarVerDatos = true;
                            }
                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                habilitarVerDatos: habilitarVerDatos
                            }

                            //  if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento)
                            listaPoliticasProy.push(politicasProyecto);
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                    }
                });
        }

        function BuscarValoresCategoria(PoliticaId, CategoriaId) {
            let tieneValor = false;
            let suma = 0;
            let sumametas = 0;

            if (vm.listaPoliticasCategorias) {
                var politica = vm.listaPoliticasCategorias.Politicas.find(p => p.PoliticaId == PoliticaId);

                if (politica) {
                    if (politica.categorias != null) {
                        var categorias = politica.Categorias.filter(c => c.CategoriaId == CategoriaId);

                        if (categorias) {
                            categorias.forEach(c => {
                                c.Fuentes.forEach(f => {
                                    f.Productos.forEach(p => {
                                        p.Localizaciones.forEach(l => {
                                            l.FocalizacionAjustada.forEach(la => {
                                                suma += la.EnAjuste;
                                                sumametas += la.MetaCategoria;
                                            });
                                        });
                                    });
                                });
                            });
                        }
                    }
                    
                }
            }

            if (suma > 0 && sumametas > 0) {
                tieneValor = true;
            }
            return tieneValor;
        }

        // Manuel ------------------------------------------------------
        vm.ConsultarPoliticasCategoriasIndicadores = function (bpin) {

            var idInstancia = $sessionStorage.idInstancia;
            var idInstanciaProyecto = $sessionStorage.idInstancia;
            return focalizacionAjustesSgrServicio.ConsultarPoliticasCategoriasIndicadores(idInstanciaProyecto, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data == "") vm.listaPoliticasCategoriasIndicadores = []
                    else {
                        var arreglo = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglo);
                        var arregloDatosGenerales = arregloGeneral.Politicas;
                        vm.habilitaVerPoliticasIndicadores = 0;
                        vm.lblIndicadores = "";
                        if (respuesta.data != null && respuesta.data != "") {

                            for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                                if (arregloDatosGenerales[pl].PoliticaId == 7) {
                                    vm.lblIndicadores1 = "Indicadores ODS y PND";
                                    vm.habilitaVerPoliticasIndicadores = 1;
                                }
                                if (arregloDatosGenerales[pl].PoliticaId == 4) {
                                    vm.lblIndicadores2 = "Indicadores PMI";
                                    vm.habilitaVerPoliticasIndicadores = 1;
                                }
                            }

                            vm.listaPoliticasCategoriasIndicadores = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        }
                    }
                });
        }

        $scope.$watch('vm.listaPoliticasProyectos', function () {
            if (vm.listaPoliticasProyectos !== undefined && vm.listaPoliticasProyectos !== null && vm.listaPoliticasProyectos.length > 0) {
                vm.listaPoliticasProyectos.forEach(politica => {
                    politica.hasIndicadores = false;
                });
            }
        });

        $scope.$watch('vm.listaPoliticasCategoriasIndicadores', function () {
            if (vm.listaPoliticasCategorias !== null && vm.listaPoliticasCategorias.Politicas.length > 0) {
                $timeout(function () {
                    ocultarPoliticasCategorias();
                }, 500);
            }

            if (vm.listaPoliticasProyectos !== undefined && vm.listaPoliticasProyectos !== null && vm.listaPoliticasProyectos.length > 0) {
                vm.listaPoliticasProyectos.forEach(politica => {
                    if (typeof vm.listaPoliticasCategoriasIndicadores.Politicas !== 'undefined' && vm.listaPoliticasCategoriasIndicadores.Politicas.find(x => x.PoliticaId == politica.politicaId).Categorias[0].ListaIndicadores !== null) {
                        politica.hasIndicadores = true;
                    }
                });
            }

            if (vm.listaPoliticasCategoriasIndicadores !== null && vm.listaPoliticasCategoriasIndicadores.Politicas != null && vm.listaPoliticasCategoriasIndicadores.Politicas.length > 0) {
                $timeout(function () {
                    ocultarPoliticasIndicadores();
                }, 500);
            }
        });

        function abrirModalAgregarIndicador(CategoiaId1, CategoriaId2, Subcategoria, Categoria, Politica, FocalizacionId) {
            var CategoriaSelec = CategoriaId2;
            var nombreCategoriaSelec = Categoria;
            var nombreSubCategoriaSelec = Subcategoria;
            var nombrePolitica = Politica;

            if (CategoriaId2 == null || CategoriaId2 == 0) {
                CategoriaSelec = CategoiaId1;
            }
            $sessionStorage.CategoriaSelec = CategoriaSelec;
            $sessionStorage.nombreCategoriaSelec = Categoria;
            $sessionStorage.nombreSubCategoriaSelec = Subcategoria;
            $sessionStorage.nombrePolitica = Politica;
            $sessionStorage.FocalizacionId = FocalizacionId;

            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/Modal/modalAgregarIndicador.html',
                controller: 'modalAgregarIndicadorController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia"
            }).result.then(function (result) {
                //vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                init();
            }, function (reason) {
                init();
            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
        }

        vm.eliminarIndicador = function (IndicadorId, Categoria2, FocalizacionId) {

            var ArregloIndicadores = [{
                IndicadorId: IndicadorId,
                Indicador: null,
                ProyectoId: vm.proyectoId,
                CategoriaId: Categoria2,
                FocalizacionIndicadorId: 0, //se envia en cero porque el post no lo usa
                Accion: "Delete"
            }]


            var ArregloCategorias = [{
                FocalizacionId: FocalizacionId
                , ListaIndicadores: ArregloIndicadores

            }]
            var ArregloPoliticas = [{
                PoliticaId: null
                , Politica: null
                , Categorias: ArregloCategorias
            }]
            var indicadoresCategoriasGuardar = {
                ProyectoId: vm.proyectoId
                , Politicas: ArregloPoliticas

            };

            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                focalizacionAjustesSgrServicio.ModificarPoliticasCategoriasIndicadores(indicadoresCategoriasGuardar, usuarioDNP, vm.idInstancia).then(function (response) {
                    if (response.statusText == "OK" && response.data) {
                        var respuestaExito = JSON.parse(response.data.toString()).Exito;
                        var respuestaMensaje = JSON.parse(response.data.toString()).Mensaje;
                        if (respuestaExito) {
                            parent.postMessage("cerrarModal", "*");
                            utilidades.mensajeSuccess("El indicador fue eliminado con éxito!", false, false, "El indicador fue eliminado con éxito!");
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            init();
                        } else {
                            swal('', respuestaMensaje, 'error');
                            init();
                        }
                    }
                });
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, null, null, "El indicador asociado sera eliminado");

        }

        vm.obtenerPoliticasTransversalesCategorias = function (bpin) {
            var idInstancia = $sessionStorage.idNivel;
            var idInstanciaProyecto = $sessionStorage.idInstancia;
            return focalizacionAjustesSgrServicio.obtenerPoliticasTransversalesCategorias(idInstanciaProyecto, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.listaPoliticasCategorias = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.Focalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.existeFocalizacion = true;
                        ValidarVerDescarDocumentos();
                        $timeout(function () {
                            guardaValoresOriginales();
                        }, 500);
                    }
                    else {
                        vm.existeFocalizacion = false;
                    }
                });
        }

        function ocultarPoliticasCategorias() {
            vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                if (document.getElementById('politicasIndicadores') != null) {
                    document.getElementById("politica-" + politicas.PoliticaId).style.display = "none";
                    document.getElementById("politica-" + politicas.PoliticaId + "-titulo").style.display = "none";
                }
            });
        }

        function ocultarPoliticasIndicadores() {
            vm.listaPoliticasCategoriasIndicadores.Politicas.forEach(politicas => {
                if (document.getElementById('politicasCategoriasIndicadores') != null) {
                    document.getElementById("politicaI-" + politicas.PoliticaId).style.display = "none";
                }
            });
        }

        function obtenerIndicador(indicadores, principal) {
            let indicador = "";

            for (let indicadorAux of indicadores) {

                if (principal == 1 && indicadorAux.TipoIndicador == "Principal") {
                    indicador = indicadorAux.NombreIndicador;
                    break;
                }

                if (principal == 0 && indicadorAux.TipoIndicador != "Principal") {
                    indicador = indicadorAux.NombreIndicador;
                    break;
                }
            }

            return indicador;
        }

        function obtenerIndicadorAcumulable(indicadores, principal) {
            let acumula = "NO";

            for (let indicadorAux of indicadores) {

                if (principal == 1 && indicadorAux.TipoIndicador == "Principal") {

                    if (indicadorAux.Acumulable == 1) {
                        acumula = "SI";
                    }

                    break;
                }

                if (principal == 0 && indicadorAux.TipoIndicador != "Principal") {

                    if (indicadorAux.Acumulable == 1) {
                        acumula = "SI";
                    }

                    break;
                }
            }

            return acumula;
        }

        function obtenerUnidadMedidaIndicadorSecundario(indicadores) {
            let unidad = "";
            for (let indicadorAux of indicadores) {
                if (indicadorAux.TipoIndicador != "Principal") {
                    unidad = indicadorAux.UnidadMedida;
                }
            }

            return unidad;
        }

        function obtenerMetaIndicadorSecundario(indicadores) {
            let meta = "";
            for (let indicadorAux of indicadores) {
                if (indicadorAux.TipoIndicador != "Principal") {
                    meta = indicadorAux.Meta;
                    break;
                }
            }

            return meta;
        }

        function obtenerAcumulativoIndicadorSecundario(indicadores) {
            let acumulativo = "";
            for (let indicadorAux of indicadores) {
                if (indicadorAux.TipoIndicador != "Principal") {
                    acumulativo = indicadorAux.Acumulable == 0 ? "No" : "Si";
                    break;
                }
            };

            return acumulativo;
        }

        //--- Funciones para el manejo de la actualización de los valores de las focalizaciones.

        vm.habilitarEditar = function (producto, fuente, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
            localizacion.HabilitaEditarLocalizador = true;
            $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).attr('disabled', false);
        }

        vm.cancelarEdicion = function (politica, categoria, fuente, producto, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
            var idInstancia = $sessionStorage.idNivel;
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                localizacion.HabilitaEditarLocalizador = false;
                $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).attr('disabled', true);
                asignarValoresOriginales(politica, categoria, fuente, producto, localizacion);
                return focalizacionAjustesSgrServicio.obtenerPoliticasTransversalesProyecto(vm.BPIN, usuarioDNP, idInstancia).then(
                    function (respuesta) {
                        utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.")
                    });
            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, 'Aceptar', 'Cancelar', 'Los posibles datos que haya diligenciado en la tabla "Focalización ajustada" se perderán.');

        }

        vm.actualizaFila = function (localizacion, fuente, productoId, vigencia, politica, categoriaId, indexPoliticas, indexCategorias) {
            calcularTotales(localizacion, fuente, productoId, vigencia, politica, categoriaId, vm.IndicePoliticaEtiquetaError, vm.IndiceCategoriaEtiquetaError);
        }

        vm.mostrarBotones = function (origen, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
            switch (origen) {
                case 1:
                    {
                        if (!vm.disabled) {
                            $("#Editar" + localizacion.LocalizacionId + '-' + indexPoliticas + '-' + indexCategorias + '-' + indexFuentes + '-' + indexProducto).attr('disabled', false);
                            $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).fadeIn();
                        }
                        break;
                    }
                case 2:
                    {
                        $("#Editar" + localizacion.LocalizacionId + '-' + indexPoliticas + '-' + indexCategorias + '-' + indexFuentes + '-' + indexProducto).attr('disabled', true);
                        $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).fadeOut();
                        break;
                    }
            }
        }

        function asignarValoresOriginales(politica, categoria, fuente, producto, localizacion) {
            var localizacionId = localizacion.LocalizacionId
            vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                if (politicas.PoliticaId == politica) {
                    politicas.Categorias.forEach(categorias => {
                        if (categorias.CategoriaId == categoria) {
                            categorias.Fuentes.forEach(fuentes => {
                                if (fuentes.FuenteId == fuente) {
                                    fuentes.Productos.forEach(productos => {
                                        if (productos.ProductoId == producto) {
                                            productos.Localizaciones.forEach(localizaciones => {
                                                if (localizaciones.LocalizacionId == localizacionId) {
                                                    localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {
                                                        focalizacionajustada.EnAjuste = focalizacionajustada.EnAjusteOriginal;
                                                        focalizacionajustada.MetaCategoria = focalizacionajustada.MetaCategoriaOriginal;
                                                        focalizacionajustada.PersonasCategoria = focalizacionajustada.PersonasCategoriaOriginal;
                                                        focalizacionajustada.MetaIndicadorSecundario = focalizacionajustada.MetaIndicadorSecundarioOriginal;
                                                    });
                                                }
                                            });
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });
            calcularTotales(localizacion, null, null, null, null, null, null, null);
        }

        function guardaValoresOriginales() {
            var esEquidadMujer = false;
            var cuantificaBeneficiarios = false;
            for (var lpc = 0; lpc < vm.listaPoliticasCategorias.Politicas.length; lpc++) {
                if (vm.listaPoliticasCategorias.Politicas[lpc].Politica === 'Equidad de la mujer') {
                    esEquidadMujer = true;
                }

                if (vm.listaPoliticasCategorias.Politicas[lpc].CuantificaBeneficiarios) {
                    cuantificaBeneficiarios = true;
                }
                if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias != null) {
                    for (var cats = 0; cats < vm.listaPoliticasCategorias.Politicas[lpc].Categorias.length; cats++) {
                        if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes != null) {
                            for (var fts = 0; fts < vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes.length; fts++) {
                                if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos != null) {
                                    for (var prd = 0; prd < vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos.length; prd++) {
                                        if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones != null) {
                                            for (var loc = 0; loc < vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones.length; loc++) {
                                                if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada != null) {
                                                    for (var laj = 0; laj < vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada.length; laj++) {
                                                        vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].cuantificaBeneficiarios = cuantificaBeneficiarios;
                                                        vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].esEquidadMujer = esEquidadMujer;
                                                        vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].EnAjusteOriginal = vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].EnAjuste;
                                                        vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].MetaCategoriaOriginal = vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].MetaCategoria;
                                                        vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].PersonasCategoriaOriginal = vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].PersonasCategoria;
                                                        vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].MetaIndicadorSecundarioOriginal = vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc].FocalizacionAjustada[laj].MetaIndicadorSecundario;
                                                    }
                                                }
                                                calcularTotales(vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].Localizaciones[loc], vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts], vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].Fuentes[fts].Productos[prd].ProductoId, null, vm.listaPoliticasCategorias.Politicas[lpc], vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].CategoriaId, 0, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                esEquidadMujer = false;
                cuantificaBeneficiarios = false;
            }
        }

        function calcularTotales(localizacion, fuente, productoId, vigencia, politica, categoriaId, indexPoliticas, indexCategorias) {
            var valorTotalEnAjuste = 0;
            var valorTotalMetaCategoria = 0;
            var valorTotalPersonasCategoria = 0;
            var valorTotalMetaIndicadorSecundario = 0;

            for (var laj = 0; laj < localizacion.FocalizacionAjustada.length; laj++) {
                valorTotalEnAjuste += localizacion.FocalizacionAjustada[laj].EnAjuste == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].EnAjuste);
                valorTotalMetaCategoria += localizacion.FocalizacionAjustada[laj].MetaCategoria == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].MetaCategoria);
                valorTotalPersonasCategoria += localizacion.FocalizacionAjustada[laj].PersonasCategoria == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].PersonasCategoria);
                valorTotalMetaIndicadorSecundario += localizacion.FocalizacionAjustada[laj].MetaIndicadorSecundario == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].MetaIndicadorSecundario);
            }
            localizacion.valorTotalEnAjuste = valorTotalEnAjuste;
            localizacion.valorTotalMetaCategoria = valorTotalMetaCategoria;
            localizacion.valorTotalPersonasCategoria = valorTotalPersonasCategoria;
            localizacion.valorTotalMetaIndicadorSecundario = valorTotalMetaIndicadorSecundario;
            var mensajeFinal = "";

            if (fuente != null && valorTotalEnAjuste > limpiaNumero(fuente.TotalFuente)) {
                mensajeFinal = `La sumatoria de los recursos focalizados en los productos por vigencia para la política ${politica.Politica} no puede superar el total de la fuente`;
            }
            else {
                if (fuente != null && limpiaNumero(fuente.TotalFocalizadoFuente) > limpiaNumero(fuente.TotalFuente)) {
                    mensajeFinal = `La sumatoria de los recursos focalizados en los productos por vigencia para la política ${politica.Politica} no puede superar el total de la fuente`;
                }
            }

            

            if (mensajeFinal != "") {
                $("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', false);
                $("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeIn();
                $("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', false);
                $("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeIn();
            } else {

                if (politica != null) {
                    $("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', true);
                    $("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeOut();
                    $("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', true);
                    $("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeOut();
                }
                
            }
            if (politica != null) {
                var errormsn = document.getElementById("errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias);
                if (errormsn != undefined) {
                    errormsn.innerHTML = '<span>' + mensajeFinal + "</span>";
                }
            }
            
        }

        //--- Guardado de valores ajustados para focalización
        vm.GuardarAjustes = function (politicaId, categoriaId, fuente, producto, localizacionId, focalizacionAjustada) {
            var a = 0;

            utilidades.mensajeWarning("Los valores de las columnas 'Solicitada $', 'Meta Categoria', 'Personas Categoria' y 'Meta Indicador Secundario' van a ser guardados, ¿desea continuar?", function funcionContinuar() {

                vm.FocalizacionArchivo = [];

                var fuenteId = fuente.FuenteId;
                var productoId = producto.ProductoId;
                var valorTotalProducto = producto.TotalCostoProducto;
                var valorTotalFuente = fuente.TotalFuente;

                focalizacionAjustada.forEach(vi => {
                    //vi.MetaEnAjuste = 0;
                    //vi.EnAjuste = 0;
                    //vi.MetaEnFirme = 0;
                    //vi.EnFirme = 0;

                    var valoresarchivo = {
                        ProyectoId: vm.Focalizacion.ProyectoId,
                        Bpin: vm.Focalizacion.BPIN,
                        PoliticaId: politicaId,
                        CategoriaId: categoriaId,
                        FuenteId: fuenteId,
                        ProductoId: productoId,
                        LocalizacionId: localizacionId,
                        Vigencia: vi.Vigencia,
                        TotalFuene: valorTotalFuente,
                        TotalCostoProducto: valorTotalProducto,
                        EnAjuste: vi.EnAjuste,
                        MetaCategoria: vi.MetaCategoria,
                        PersonasCategoria: vi.PersonasCategoria,
                        MetaIndicadorSecundario: vi.MetaIndicadorSecundario
                    };

                    vm.FocalizacionArchivo.push(valoresarchivo);
                    //calcularTotales(ProductoId, fuenteId, localizacion, vigencias, 0);
                });
                GuardarArchivoFocalizacion(false, 0);
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            });

        }

        //--- Eliminar categorías

        function eliminarCategoriaPolitica(politicaId, politica, categoria) {

            var idInstancia = $sessionStorage.idNivel;
            var proyectoId = $sessionStorage.idProyectoEncabezado;
            var categoriaId = categoria.CategoriaId;

            var mensaje = 'Se perderá los datos focalizados en las localizaciones de la categoría: ' + categoria.Categoria;
            if (categoria.SubCategoria !== null) {
                mensaje += ' y subcategoría: ' + categoria.SubCategoria + '.';
            }

            utilidades.mensajeWarning(mensaje + ' ¿Está seguro de continuar?', function funcionContinuar() {
                const mensaje3 = "Los datos fueron eliminados con éxito.";
                return focalizacionAjustesSgrServicio.eliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId, usuarioDNP, idInstancia).then(
                    function (respuesta) {

                        let exito = respuesta.data;
                        if (exito = 'OK') {
                            guardarCapituloModificado();
                            new utilidades.mensajeSuccess("", false, false, false, mensaje3);
                        } else {
                            new utilidades.mensajeError("No se puede eliminar la politica seleccionada, se encuentra con valores en firme.");
                        }
                    });
            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, null, null, 'Los datos serán eliminados.');
        }

        function ValidarVerDescarDocumentos() {
            vm.Focalizacion.Politicas.forEach(politicas => {
                if (politicas.Categorias == null)
                    vm.existeFocalizacion = false;
            });
        }

        function abrirMensajeInformacionRegionalizacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Las categorías sobre las que ya registró información sobre su ejecución no se encuentran disponibles para ser eliminadas.</span>");
        }

        function abrirMensajeMetaCategoria() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <br /> <br /> <span class='tituhori' > El valor del campo ''Meta categoría'' debe ser menor o igual a la meta del indicador principal del producto por vigencia.</span>");
        }

        function abrirMensajeMetaIndicador() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > El valor del campo ''Meta indicador secundario'' debe ser menor o igual a la meta del indicador secundario del producto que tiene la marca de equidad de la mujer, por vigencia.</span>");
        }

        function ConvertirNumero2decimales(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
        function ConvertirNumero4decimales(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }
        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 0,
            }).format(numero);
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.obtenerPoliticasTransversalesCategorias(vm.BPIN);
                vm.ConsultarPoliticasCategoriasIndicadores(vm.BPIN);
            }
            if (vm.componentesRefreshEliminar.includes(nombreCapituloHijo)) {
                eliminarCapitulosModificados();
            }
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }

        function recorrerObjetivosNumber(event) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));
        }

        vm.getNombreReducido = function (data, maxCaracteres) {
            if (data.length > maxCaracteres) {
                var dataNueva = data.slice(0, maxCaracteres);
                return dataNueva + '...';
            } else return data
        }

        function abrirModalAgregarCategoriaPolitica(Politica) {
            console.log(Politica);
            var categoriasXPolitica = [];

            for (var lpc = 0; lpc < vm.listaPoliticasCategorias.Politicas.length; lpc++) {
                if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias != null && vm.listaPoliticasCategorias.Politicas[lpc].PoliticaId === Politica.PoliticaId) {
                    for (var cats = 0; cats < vm.listaPoliticasCategorias.Politicas[lpc].Categorias.length; cats++) {
                        categoriasXPolitica.push(vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].CategoriaId);
                    }
                }
            }

            $sessionStorage.categoriasXPolitica = categoriasXPolitica;
            //$sessionStorage.idPolitica = idPolitica;
            //$sessionStorage.nombrePoliticaCat = nombrePoliticaCat;

            var data = {
                idpolitica: Politica.PoliticaId,
                nombrePoliticaCat: Politica.Politica
            }

            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: "src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/ResumenFocalizacion/FocalizacionCategoria/modalAgregarCategoriaPolitica.html",
                controller: 'modalAgregarCategoriaPoliticaController',
                controllerAs: "vm",
                openedClass: "modal-contentDNP",
                resolve: {
                    categoria: function () {
                        return data;
                    },

                },
            });
            modalInstance.result.then(data => {
                if (data != null) {
                    guardarCapituloModificado();
                }
            });
        }

        /*Carga Masiva*/
        vm.activarControles = function (evento) {
            switch (evento) {
                case "inicio":
                    $("#btnFocalizacionValidarArchivo").attr('disabled', true);
                    $("#btnFocalizacionLimpiarArchivo").attr('disabled', true);
                    $("#btnFocalizacionArchivoSeleccionado").attr('disabled', true);
                    document.getElementById('filefocalizacion').value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnFocalizacionValidarArchivo").attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo").attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnFocalizacionValidarArchivo").attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo").attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado").attr('disabled', false);
                    break;
                default:
            }
        }

        function HabilitaEditar(band) {
            vm.HabilitaEditarBandera = band;
        }

        function exportarFocalizacionExcel() {

            utilidades.mensajeWarning("Si ocurren inconvenientes de descarga o visualización, es necesario actualizar la aplicación.", function funcionContinuar() {

                const filename = 'Template_.xlsx';
                const COL_PARAMS = ['hidden', 'wpx', 'width', 'wch', 'MDW'];
                const STYLE_PARAMS = ['fill', 'font', 'alignment', 'border'];
                var styleConf = {
                    'E4': {
                        fill: { fgColor: { rgb: 'FFFF0000' } }
                    }
                }

                var columns = [
                    {
                        name: 'ProyectoId', title: 'Proyecto Id'
                    },
                    {
                        name: 'Bpin', title: 'Bpin'
                    },
                    {
                        name: 'PoliticaId', title: 'Politica Id'
                    },
                    {
                        name: 'CategoriaId', title: 'Categoria Id'
                    },
                    {
                        name: 'FuenteId', title: 'Fuente Id'
                    },
                    {
                        name: 'ProductoId', title: 'Producto Id'
                    },
                    {
                        name: 'LocalizacionId', title: 'Localizacion Id'
                    },
                    {
                        name: 'Politica', title: 'Politica'
                    },
                    {
                        name: 'Categoria', title: 'Categoria'
                    },
                    {
                        name: 'SubCategoria', title: 'SubCategoria'
                    },
                    {
                        name: 'Fuente', title: 'Fuente'
                    },
                    {
                        name: 'Etapa', title: 'Etapa'
                    },
                    {
                        name: 'TipoFinanciador', title: 'Tipo Financiador'
                    },
                    {
                        name: 'Financiador', title: 'Financiador'
                    },
                    {
                        name: 'Recurso', title: 'Recurso'
                    },
                    {
                        name: 'TotalFuente', title: 'Total Fuente'
                    },
                    {
                        name: 'Producto', title: 'Producto'
                    },
                    {
                        name: 'TotalCostoProducto', title: 'Total Costo Producto'
                    },
                    {
                        name: 'Localizacion', title: 'Localizacion'
                    },
                    {
                        name: 'Vigencia', title: 'Vigencia'
                    },
                    {
                        name: 'EnAjuste', title: 'En Ajuste $'
                    },
                    {
                        name: 'MetaCategoria', title: 'Meta Categoria'
                    },
                    {
                        name: 'PersonasCategoria', title: 'Personas Categoria'
                    },
                    {
                        name: 'MetaIndicadorSecundario', title: 'Meta Indicador Secundario'
                    },
                    {
                        name: 'CuantificaPersonasCategoria', title: 'CuantificaPersonasCategoria'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla Ajuste Focalizacion Categorias",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Focalizacion Categorias");

                const header = colNames;
                const data = [];

                vm.Focalizacion.Politicas.forEach(politicas => {
                    if (politicas.TieneConceptoPendiente == false) {
                        politicas.Categorias.forEach(categorias => {
                            categorias.Fuentes.forEach(fuentes => {
                                fuentes.Productos.forEach(productos => {
                                    productos.Localizaciones.forEach(localizaciones => {
                                        localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {

                                            data.push({
                                                ProyectoId: vm.Focalizacion.ProyectoId,
                                                Bpin: vm.Focalizacion.BPIN,
                                                PoliticaId: politicas.PoliticaId,
                                                CategoriaId: categorias.CategoriaId,
                                                FuenteId: fuentes.FuenteId,
                                                ProductoId: productos.ProductoId,
                                                LocalizacionId: localizaciones.LocalizacionId,
                                                Politica: politicas.Politica,
                                                Categoria: categorias.Categoria,
                                                SubCategoria: categorias.SubCategoria,
                                                Fuente: fuentes.TipoFinanciador + " " + fuentes.Financiador + " " + fuentes.Recurso,
                                                Etapa: fuentes.Etapa,
                                                TipoFinanciador: fuentes.TipoFinanciador,
                                                Financiador: fuentes.Financiador,
                                                Recurso: fuentes.Recurso,
                                                TotalFuente: fuentes.TotalFuente,
                                                Producto: productos.NombreProducto,
                                                TotalCostoProducto: productos.TotalCostoProducto,
                                                Localizacion: localizaciones.Localizacion,
                                                Vigencia: focalizacionajustada.Vigencia,
                                                EnAjuste$: parseFloat(focalizacionajustada.EnAjuste).toFixed(2),
                                                MetaCategoria: parseFloat(focalizacionajustada.MetaCategoria).toFixed(4),
                                                PersonasCategoria: parseFloat(focalizacionajustada.PersonasCategoria),
                                                MetaIndicadorSecundario: parseFloat(focalizacionajustada.MetaIndicadorSecundario).toFixed(4),
                                                CuantificaPersonasCategoria: politicas.CuantificaPersonasCategoria
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    }
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: ["ProyectoId",
                        "Bpin",
                        "PoliticaId",
                        "CategoriaId",
                        "FuenteId",
                        "ProductoId",
                        "LocalizacionId",
                        "Politica",
                        "Categoria",
                        "SubCategoria",
                        "Fuente",
                        "Etapa",
                        "TipoFinanciador",
                        "Financiador",
                        "Recurso",
                        "TotalFuente",
                        "Producto",
                        "TotalCostoProducto",
                        "Localizacion",
                        "Vigencia",
                        "EnAjuste$",
                        "MetaCategoria",
                        "PersonasCategoria",
                        "MetaIndicadorSecundario",
                        "CuantificaPersonasCategoria"]
                });

                for (let col of [15]) {
                    formatColumn(worksheet, col, "#,##")
                }

                for (let col of [17]) {
                    formatColumn(worksheet, col, "#,##")
                }

                /* hide second column */
                worksheet['!cols'] = [];

                worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };
                worksheet['!cols'][4] = { hidden: true };
                worksheet['!cols'][5] = { hidden: true };
                worksheet['!cols'][6] = { hidden: true };
                worksheet['!cols'][24] = { hidden: true };

                wb.Sheets["Focalizacion Categorias"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaAjusteFocalizacionCategorias.xlsx');
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function formatColumn(worksheet, col) {
            var fmtnumero2 = "##,##";
            var fmtnumero4 = "#,####";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })
                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "S1") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "T1") {
                    worksheet[ref].z = fmtnumero4;
                    worksheet[ref].t = 'n';
                }
            }
        }

        function s2ab(s) {
            var buf = new ArrayBuffer(s.length); //convert s to arrayBuffer
            var view = new Uint8Array(buf);  //create uint8array as viewer
            for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF; //convert to octet
            return buf;
        }

        function adjuntarArchivo() {
            document.getElementById('filefocalizacion').value = "";
            document.getElementById('filefocalizacion').click();
        }

        $scope.filefocalizacionNameChanged = function (input) {
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                vm.activarControles('cargaarchivo');
            }
            else {
                //vm.filename = input.files.length + " archivos"               
                vm.activarControles('inicio');
            }
        }

        $scope.ChangeFocalizacionSet = function () {
            if (vm.nombrearchivo == "") {

            }
        };

        function limpiarArchivo() {
            $scope.filesfocalizacion = [];
            document.getElementById('filefocalizacion').value = "";
            vm.activarControles('inicio');
        }

        function HabilitaEditar(band) {

            vm.HabilitaEditarBandera = band;
        }

        function validarArchivo() {
            var resultado = true;
            var enajuste = 0;
            var metaCategoria = 0;
            var personasCategoria = 0;
            var metaIndicadorSecundario = 0;
            vm.FocalizacionArchivo = [];
            if (filefocalizacion.files.length > 0) {

                let file = document.getElementById("filefocalizacion").files[0];
                if ($scope.validaFocalizacionNombreArchivo(file.name)) {
                    if (typeof (FileReader) != "undefined") {
                        var reader = new FileReader();
                        if (reader.readAsBinaryString) {
                            reader.onload = function (e) {
                                var workbook = XLSX.read(e.target.result, {
                                    type: 'binary'
                                });
                                var firstSheet = workbook.SheetNames[0];
                                var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
                                resultado = excelRows.map(function (item, index) {

                                    if (item["ProyectoId"] == undefined) {
                                        utilidades.mensajeError("La columna ProyectoId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["ProyectoId"])) {
                                        utilidades.mensajeError("El valor ProyectoId " + item["ProyectoId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["Bpin"] == undefined) {
                                        utilidades.mensajeError("La columna Bpin no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Bpin"])) {
                                        utilidades.mensajeError("El valor Bpin " + item["Bpin"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["ProductoId"] == undefined) {
                                        utilidades.mensajeError("La columna ProductoId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["ProductoId"])) {
                                        utilidades.mensajeError("El valor ProductoId " + item["ProductoId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["FuenteId"] == undefined) {
                                        utilidades.mensajeError("La columna FuenteId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["FuenteId"])) {
                                        utilidades.mensajeError("El valor FuenteId " + item["FuenteId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["LocalizacionId"] == undefined) {
                                        utilidades.mensajeError("La columna LocalizacionId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["LocalizacionId"])) {
                                        utilidades.mensajeError("El valor LocalizacionId " + item["LocalizacionId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["Vigencia"] == undefined) {
                                        utilidades.mensajeError("La columna Vigencia no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Vigencia"])) {
                                        utilidades.mensajeError("El valor de la Vigencia " + item["Vigencia"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["PoliticaId"] == undefined) {
                                        utilidades.mensajeError("La columna PoliticaId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["PoliticaId"])) {
                                        utilidades.mensajeError("El valor PoliticaId " + item["PoliticaId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["CategoriaId"] == undefined) {
                                        utilidades.mensajeError("La columna CategoriaId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["CategoriaId"])) {
                                        utilidades.mensajeError("El valor CategoriaId " + item["CategoriaId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["EnAjuste$"] == undefined) {
                                        utilidades.mensajeError("La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
                                        return false;
                                    }
                                    //else if (typeof (item["EnAjuste$"]) != "number") {
                                    //	utilidades.mensajeError("Valor no valido 'En ajuste $' " + item["EnAjuste$"] + ". La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
                                    //	return false;
                                    //}
                                    else if (!ValidarDicimal(item["EnAjuste$"].toString(), 2)) {
                                        utilidades.mensajeError("Valor no valido 'En ajuste $' " + item["EnAjuste$"] + ". La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        enajuste = item["EnAjuste$"];
                                    }

                                    if (item["MetaCategoria"] == undefined) {
                                        utilidades.mensajeError("La columna 'Meta MetaCategoria' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    //else if (typeof (item["MetaCategoria"]) != "number") {
                                    //	utilidades.mensajeError("Valor no valido 'Meta MetaCategoria' " + item["MetaCategoria"] + ". La columna 'Meta MetaCategoria' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                    //	return false;
                                    //}
                                    else if (!ValidarDicimal(item["MetaCategoria"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'Meta MetaCategoria' " + item["MetaCategoria"] + ". La columna 'Meta MetaCategoria' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        metaCategoria = item["MetaCategoria"];
                                    }

                                    if (item["PersonasCategoria"] == undefined) {
                                        utilidades.mensajeError("La columna 'Personas Categoria' solo acepta valores numericos sin decimales");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["PersonasCategoria"].toString(), 0)) {
                                        utilidades.mensajeError("Valor no valido 'Personas Categoria' " + item["PersonasCategoria"] + ". La columna 'Personas Categoria' solo acepta valores numericos sin decimales");
                                        return false;
                                    }
                                    else {
                                        personasCategoria = item["PersonasCategoria"];
                                    }

                                    if (item["MetaIndicadorSecundario"] == undefined) {
                                        utilidades.mensajeError("La columna 'Meta Indicador Secundario' solo acepta valores numericos sin decimales");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["MetaIndicadorSecundario"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'Meta Indicador Secundario' " + item["MetaIndicadorSecundario"] + ". acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarMetaIndicadorSecundarioPolitica(item["MetaIndicadorSecundario"], item["PoliticaId"])) {
                                        utilidades.mensajeError("Valor no valido 'Meta Indicador Secundario' " + item["MetaIndicadorSecundario"] + ". La columna 'Meta Indicador Secundario' solo acepta valores mayores a cero para la política 'Equidad de la mujer'");
                                        return false;
                                    }
                                    else {
                                        metaIndicadorSecundario = item["MetaIndicadorSecundario"];
                                    }

                                    if (!ValidarCuantificaPersonasCategoria(item["CuantificaPersonasCategoria"], item["PersonasCategoria"])) {
                                        utilidades.mensajeError("El campo 'Personas categoría', valor del campo: " + item["PersonasCategoria"] + ", no aplica para las políticas: " + vm.Focalizacion.PoliticasNoCuentificanPersonas[0].PolicitcasNoCuantifica);
                                        return false;
                                    }


                                    var valoresarchivo = {
                                        ProyectoId: item["ProyectoId"],
                                        Bpin: item["Bpin"],
                                        PoliticaId: item["PoliticaId"],
                                        CategoriaId: item["CategoriaId"],
                                        FuenteId: item["FuenteId"],
                                        ProductoId: item["ProductoId"],
                                        LocalizacionId: item["LocalizacionId"],
                                        Vigencia: item["Vigencia"],
                                        TotalFuene: item["TotalFuente"],
                                        TotalCostoProducto: item["TotalCostoProducto"],
                                        EnAjuste: enajuste,// item["EnAjuste"],
                                        MetaCategoria: metaCategoria,//item["MetaEnAjuste"],
                                        PersonasCategoria: personasCategoria,
                                        MetaIndicadorSecundario: metaIndicadorSecundario
                                    };
                                    vm.FocalizacionArchivo.push(valoresarchivo);

                                });

                                if (resultado.indexOf(false) == -1) {
                                    if (!ValidarRegistros()) {
                                        utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
                                        vm.activarControles('inicio');
                                    }
                                    else {
                                        vm.activarControles('validado');
                                        utilidades.mensajeSuccess("El campo 'Meta indicador secundario' solo aplica para la política Equidad de la mujer. Proceda a cargar el archivo para que quede registrado en el sistema", false, false, false, "Validacion de Carga Exitosa.");
                                    }
                                }
                                else {
                                    vm.activarControles('inicio');
                                    vm.FocalizacionArchivo = [];
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
        }

        function ValidarRegistros() {

            var aPolicitas = [];
            var aCategorias = [];
            var aFuentes = [];
            var aProductos = [];
            var aLocalizaciones = [];
            var aVigencias = [];

            var existeproyecto = 0;
            var existeBpin = 0;
            var existePolitica = 0;
            var existeCategoria = 0;
            var existeFuente = 0;
            var existeProducto = 0;
            var existeLocaliacion = 0;
            var existeVigencia = 0;
            var existePeriodoProyecto = 0;
            var CantidadRegistros = 0;


            vm.Focalizacion.Politicas.forEach(politicas => {
                aPolicitas.push(politicas.PoliticaId);
                politicas.Categorias.forEach(categorias => {
                    aCategorias.push(categorias.CategoriaId);
                    categorias.Fuentes.forEach(fuentes => {
                        aFuentes.push(fuentes.FuenteId);
                        fuentes.Productos.forEach(productos => {
                            aProductos.push(productos.ProductoId);
                            productos.Localizaciones.forEach(localizaciones => {
                                aLocalizaciones.push(localizaciones.LocalizacionId);
                                localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {
                                    aVigencias.push(focalizacionajustada.Vigencia);
                                    CantidadRegistros = CantidadRegistros + 1;
                                });

                            });
                        });
                    });
                });
            });

            vm.FocalizacionArchivo.forEach(fa => {

                if (fa.ProyectoId != vm.Focalizacion.ProyectoId) {
                    existeproyecto++;
                }
                if (fa.Bpin != vm.Focalizacion.BPIN) {
                    existeBpin++;
                }

                if (aPolicitas.indexOf(fa.PoliticaId) == -1) {
                    existePolitica = existePolitica + 1;
                }
                if (aCategorias.indexOf(fa.CategoriaId) == -1) {
                    existeCategoria = existeCategoria + 1;
                }

                if (aFuentes.indexOf(fa.FuenteId) == -1) {
                    existeFuente = existeFuente + 1;
                }
                if (aProductos.indexOf(fa.ProductoId) == -1) {
                    existeProducto = existeProducto + 1;
                }
                if (aLocalizaciones.indexOf(fa.LocalizacionId) == -1) {
                    existeLocaliacion = existeLocaliacion + 1;
                }
                if (aVigencias.indexOf(fa.Vigencia) == -1) {
                    existeVigencia = existeVigencia + 1;
                }
            });


            if (existeproyecto > 0 || existeCategoria > 0 || existeBpin > 0 || existeFuente > 0 || existeProducto > 0
                || existeLocaliacion > 0 || existeVigencia > 0 || existePeriodoProyecto > 0) {
                return false;
            }
            else {
                if (CantidadRegistros != vm.FocalizacionArchivo.length) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function ValidarMetaIndicadorSecundarioPolitica(valor, PoliticaId) {
            if (PoliticaId != 7 && valor > 0) {
                return false;
            }
            return true;

        }

        function ValidarCuantificaPersonasCategoria(CuantificaPersonasCategoria, valor) {

            if (!CuantificaPersonasCategoria && valor > 0) {
                return false
            }
            return true;
        }

        function ValidarDicimal(valor, decimals) {

            //if (valor.toString().includes(',')) {
            //    return false;
            //}
            if (valor.toString().includes('.')) {
                var entero = valor.toString().split('.')[0];
                var decimal = valor.toString().split('.')[1];

                if (isNaN(entero)) {
                    return false;
                }

                if (isNaN(decimal)) {
                    return false;
                }

                if (decimal.length > decimals) {
                    return false;
                }
            }
            else {
                if (isNaN(valor)) {
                    return false;
                }
            }

            return true;

        }

        function ValidaSiEsNumero(valor) {
            if (valor === undefined)
                return false;
            else if (!isNaN(limpiaNumero(valor))) {
                return true;
            }
            else {
                return false;
            }

        }

        function limpiaNumero(valor) {
            //return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
            let numeroLimpio = valor.replace(/\./g, '').replace(',', '.');
            return parseFloat(numeroLimpio);
        }

        $scope.validaFocalizacionNombreArchivo = function (nombre) {
            var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.xls|.xlsx)$/;
            if (!regex.test(nombre.toLowerCase())) {
                utilidades.mensajeError("El archivo no es de tipo Excel!");
                $scope.files = [];
                $scope.nombreArchivo = '';
                return false;
            }
            else {
                return true;
            }
        }

        function GuardarArchivoFocalizacion(valorescero, cargamasiva) {
            if (cargamasiva)
                vm.activarControles('inicio');
            return focalizacionAjustesSgrServicio.guardarFocalizacionCategorias(vm.FocalizacionArchivo, vm.idUsuario).then(function (response) {
                //var respuesta = jQuery.parseJSON(response.data)

                if (response.statusText == "OK") {
                    if (valorescero == true) {
                        var Localizacion = null;
                        vm.Focalizacion.Politicas.forEach(politicas => {
                            if (politicas.PoliticaId == vm.FocalizacionArchivo[0].PoliticaId) {
                                politicas.Categorias.forEach(categorias => {
                                    if (categorias.CategoriaId == vm.FocalizacionArchivo[0].CategoriaId) {
                                        categorias.Fuentes.forEach(fuentes => {
                                            if (fuentes.FuenteId == vm.FocalizacionArchivo[0].FuenteId) {
                                                fuentes.Productos.forEach(productos => {
                                                    if (productos.ProductoId == vm.FocalizacionArchivo[0].ProductoId) {
                                                        productos.Localizaciones.forEach(localizaciones => {
                                                            if (localizaciones.LocalizacionId == vm.FocalizacionArchivo[0].LocalizacionId) {
                                                                localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {
                                                                    focalizacionajustada.EnAjuste = 0;
                                                                    focalizacionajustada.MetaCategoria = 0;
                                                                    focalizacionajustada.PersonasCategoria = 0;
                                                                    focalizacionajustada.MetaIndicadorSecundario = 0;
                                                                });

                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                        });

                        vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                            if (politicas.PoliticaId == vm.FocalizacionArchivo[0].PoliticaId) {
                                politicas.Categorias.forEach(categorias => {
                                    if (categorias.CategoriaId == vm.FocalizacionArchivo[0].CategoriaId) {
                                        categorias.Fuentes.forEach(fuentes => {
                                            if (fuentes.FuenteId == vm.FocalizacionArchivo[0].FuenteId) {
                                                fuentes.Productos.forEach(productos => {
                                                    if (productos.ProductoId == vm.FocalizacionArchivo[0].ProductoId) {
                                                        productos.Localizaciones.forEach(localizaciones => {
                                                            if (localizaciones.LocalizacionId == vm.FocalizacionArchivo[0].LocalizacionId) {
                                                                console.log(localizaciones.LocalizacionId);
                                                                localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {
                                                                    focalizacionajustada.EnAjuste = 0;
                                                                    focalizacionajustada.MetaCategoria = 0;
                                                                    focalizacionajustada.PersonasCategoria = 0;
                                                                    focalizacionajustada.MetaIndicadorSecundario = 0;
                                                                });
                                                                Localizacion = localizaciones;
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                        });

                        calcularTotales(Localizacion, null, null, null, null, null, null, null);
                        vm.FocalizacionArchivo = [];
                    }
                    else {
                        vm.obtenerPoliticasTransversalesCategorias(vm.BPIN);
                        //ObtenerSeccionCapitulo();
                    }
                    if (valorescero) {

                        utilidades.mensajeSuccess('', false, false, false, "Se han asignado valores en cero para la tabla de 'Focalización ajustada'.");
                    }
                    else {
                        utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                    }

                    guardarCapituloModificado();
                }
                else {
                    //var ValidacionGuardar = document.getElementById(vm.nombreComponente + "-validacionguardar-error");
                    //if (ValidacionGuardar != undefined) {
                    //    var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionguardar-error-mns");
                    //    if (ValidacionFFR1Error != undefined) {
                    //        ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                    //        ValidacionFFR1.classList.remove('hidden');
                    //    }
                    //}
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

        function valoresEnCero(politicaId, categoriaId, fuenteId, productoId, localizacionId, focalizacionAjustada) {

            utilidades.mensajeWarning("Los valores de las columnas de la tabla 'Focalización ajustada' quedarán en ceros ¿esta seguro de continuar?", function funcionContinuar() {

                vm.FocalizacionArchivo = [];

                focalizacionAjustada.forEach(vi => {
                    vi.MetaEnAjuste = 0;
                    vi.EnAjuste = 0;
                    vi.MetaEnFirme = 0;
                    vi.EnFirme = 0;

                    var valoresarchivo = {
                        ProyectoId: vm.Focalizacion.ProyectoId,
                        Bpin: vm.Focalizacion.BPIN,
                        PoliticaId: politicaId,
                        CategoriaId: categoriaId,
                        FuenteId: fuenteId,
                        ProductoId: productoId,
                        LocalizacionId: localizacionId,
                        Vigencia: vi.Vigencia,
                        TotalFuene: 0,
                        TotalCostoProducto: 0,
                        EnAjuste: 0,
                        MetaCategoria: 0,
                        PersonasCategoria: 0,
                        MetaIndicadorSecundario: 0
                    };

                    vm.FocalizacionArchivo.push(valoresarchivo);
                    //calcularTotales(ProductoId, fuenteId, localizacion, vigencias, 0);
                });
                GuardarArchivoFocalizacion(true, 0);

            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, null, null, "Algunos datos de la tabla se modificarán");

        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia
            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.mostrarMensajeError = function (valorTotal, valorAjustado) {
            var respuesta = false;
            if (valorAjustado > valorTotal)
                respuesta = true;

            return respuesta;
        }

        vm.validateFormat = function (event, cantidad) {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44 && event.keyCode != 46) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                vm.puntoDigitado = false;
                if (cantidad == 4)
                    tamanioPermitido = 16;
                else
                    tamanioPermitido = 14;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }

                }
            } else {
                if (tamanio > 12 && event.keyCode != 44 && event.keyCode != 46) {
                    event.preventDefault();
                }
            }

            if ((event.keyCode == 44 || event.keyCode == 46) && tamanio == 12) {
                vm.puntoDigitado = true;
            }
            else if (vm.puntoDigitado && tamanio == 12) {
                vm.puntoDigitado = false;
            }
            else {
                if (cantidad == 4) {
                    if (tamanio > tamanioPermitido || tamanio > 16) {
                        event.preventDefault();
                    }
                }
                else {
                    if (tamanio > tamanioPermitido || tamanio > 14) {
                        event.preventDefault();
                    }
                }

            }
        }
        vm.validarTamanio = function (event, cantidad) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");

            if (permitido) {
                vm.puntoDigitado = false;
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > cantidad) {
                }
                if (cantidad == 4)
                    tamanioPermitido = 16;
                else
                    tamanioPermitido = 14;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }
                }
            }
        }

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Categorías Políticas");
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Data, p.Descripcion);
                        });
                    }
                }

            }

            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        };

        vm.limpiarErrores = function () {
            //console.log("vm.limpiarErrores");
            if (vm.listaPoliticasProyectos !== null) {
                vm.listaPoliticasProyectos.forEach(politicas => {
                    politicas.presentaError = false;
                    politicas.presentaErrorVacio = false;
                });
            }

            if (vm.listaPoliticasCategorias !== null) {
                vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                    politicas.presentaError = false;
                    politicas.presentaErrorVacioPol = false;
                    politicas.mensajeError1Politica = '';
                    if (politicas.Categorias !== null) {
                        politicas.Categorias.forEach(categorias => {
                            categorias.presentaError1Categoria = false;
                            categorias.presentaError2Categoria = false;
                            if (categorias.Fuentes !== null) {
                                categorias.Fuentes.forEach(fuentes => {
                                    fuentes.presentaError1Fuente = false;
                                    fuentes.mensajeError1Fuente = '';
                                    fuentes.presentaError2Fuente = false;
                                    if (fuentes.Productos !== null) {
                                        fuentes.Productos.forEach(producto => {
                                            producto.presentaError1producto = false;
                                            producto.mensajeError1producto = '';
                                        });
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        vm.validarFOC_CAT_001 = function (errores, descripcion) {
            var politicaId = null, categoriaId = null, fuenteId = null;
            if (errores.length > 0) {
                var erroresArr = JSON.parse(errores);

                erroresArr.forEach(e => {
                    politicaId = e.PoliticaId;
                    categoriaId = e.CategoriaId;
                    fuenteId = e.FuenteId;

                    vm.listaPoliticasProyectos.forEach(politicas => {
                        if (politicas.politicaId == politicaId) {
                            politicas.presentaError = true;
                        }
                    });

                    if (vm.listaPoliticasCategorias !== null) {
                        vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                            if (politicas.PoliticaId == politicaId) {
                                politicas.presentaErrorVacioPol = true;
                                politicas.presentaError = true;
                                if (politicas.Categorias !== null) {
                                    politicas.Categorias.forEach(categorias => {
                                        if (categorias.CategoriaId == categoriaId) {
                                            if (categorias.Fuentes !== null) {
                                                categorias.Fuentes.forEach(fuentes => {
                                                    if (fuentes.FuenteId == fuenteId) {
                                                        fuentes.presentaError1Fuente = true;
                                                        fuentes.mensajeError1Fuente = descripcion;
                                                    }
                                                });
                                                categorias.presentaError1Categoria = true;
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    }
                });
            }
            console.log(vm.listaPoliticasProyectos);

            //var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-focFuenteProducto-error");
            //if (campoObligatorioJustificacion != undefined) {
            //	campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
            //	campoObligatorioJustificacion.classList.remove('hidden');
            //}
        }

        vm.validarFOC_CAT_002 = function (errores, descripcion) {
            var politicaId = null, categoriaId = null, fuenteId = null, productoId = null;

            if (errores.length > 0) {
                var erroresArr = JSON.parse(errores);

                erroresArr.forEach(e => {
                    politicaId = e.PoliticaId;
                    categoriaId = e.CategoriaId;
                    fuenteId = e.FuenteId;
                    productoId = e.ProductoId;

                    if (vm.listaPoliticasProyectos !== null) {
                        vm.listaPoliticasProyectos.forEach(politicas => {
                            if (politicas.politicaId == politicaId) {
                                politicas.presentaError = true;
                            }
                        });
                    }

                    if (vm.listaPoliticasCategorias !== null) {
                        vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                            if (politicas.PoliticaId == politicaId) {
                                politicas.presentaErrorVacioPol = true;
                                politicas.presentaError = true;
                                if (politicas.Categorias !== null) {
                                    politicas.Categorias.forEach(categorias => {
                                        categorias.presentaError2Categoria = true;
                                        if (categorias.CategoriaId == categoriaId) {
                                            if (categorias.Fuentes !== null) {
                                                categorias.Fuentes.forEach(fuentes => {
                                                    if (fuentes.FuenteId == fuenteId) {
                                                        if (fuentes.Productos !== null) {
                                                            fuentes.Productos.forEach(producto => {
                                                                if (producto.ProductoId == productoId) {
                                                                    producto.presentaError1producto = true;
                                                                    producto.mensajeError1producto = descripcion;
                                                                }
                                                            });
                                                            fuentes.presentaError2Fuente = true;
                                                        }
                                                    }
                                                });
                                                categorias.presentaError2Categoria = true;
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    }
                });
            }

            console.log(vm.listaPoliticasProyectos);
        }

        vm.validarFOC_CAT_003 = function (errores, descripcion) {
            var politicaId = null;

            if (errores.length > 0) {
                var erroresArr = JSON.parse(errores);

                erroresArr.forEach(e => {
                    politicaId = e.PoliticaId;

                    if (vm.listaPoliticasProyectos !== null) {
                        vm.listaPoliticasProyectos.forEach(politicas => {
                            if (politicas.politicaId == politicaId) {
                                politicas.presentaErrorVacio = true;
                            }
                        });
                    }

                    if (vm.listaPoliticasCategorias !== null) {
                        vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
                            if (politicas.PoliticaId == politicaId) {
                                politicas.presentaError = true;
                                politicas.presentaErrorVacioPol = true;
                                politicas.mensajeError1Politica = descripcion;
                            }
                        });
                    }

                });
            }
            console.log(vm.listaPoliticasProyectos);
        }

        vm.errores = {
            'FOC_CAT_001': vm.validarFOC_CAT_001,
            'FOC_CAT_002': vm.validarFOC_CAT_002,
            'FOC_CAT_003': vm.validarFOC_CAT_003,
        }
    }

    angular.module('backbone').component('resumenFocalizacionSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/ResumenFocalizacion/ResumenFocalizacionSgr.html",
        controller: ResumenFocalizacionSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&',
            notificacioninicio: '&'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();