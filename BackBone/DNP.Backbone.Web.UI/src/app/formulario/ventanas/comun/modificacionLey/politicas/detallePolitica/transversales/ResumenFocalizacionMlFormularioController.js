(function () {
    'use strict';

    resumenFocalizacionMlFormularioController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'constantesBackbone',
        '$timeout',
        'focalizacionAjustesServicio',
        'justificacionCambiosServicio',
        'comunesServicio',
        'modificacionLeyServicio'
    ];

    function resumenFocalizacionMlFormularioController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        focalizacionAjustesServicio,
        justificacionCambiosServicio,
        comunesServicio,
        modificacionLeyServicio
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
        vm.ProductoConsultado = null;
        vm.FuenteConsultada = null;
        vm.localizacionConsultada = null;
        vm.CategoriaConsultada = null;
        vm.CategoriaConsultadaFocalizada = 0;
        vm.imgmasAnt;
        vm.imgmenosAnt;
        vm.detailAnt;
        vm.BloquearCargue = false;
        vm.calendarioPoliticasTransversales = 'false';
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1        
        vm.nombreComponente = "resumenFocalizacionMlFormulario";
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.componentesRefresh = [
            'recursoscostosdelasacti',
            'datosgeneralesindicadoresdepr',            
            'focalizacionpoliticastransv',
            'focalizacionpolpoliticastransv',
            'datosgeneralesbeneficiariosTotales',
            'focalizacionpolcrucedepolitica',
        ];
        vm.currentYear = new Date().getFullYear();
        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.seccionCapitulo = null;
        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.CargarArchivoPoliticasTransversales = CargarArchivoPoliticasTransversales;
        vm.exportarFocalizacionExcel = exportarFocalizacionExcel;
        vm.ejecutarConsultaCargueMasivo = ejecutarConsultaCargueMasivo;
        vm.abrirModalAgregarCategoriaPolitica = abrirModalAgregarCategoriaPolitica;
        vm.eliminarCategoriaPolitica = eliminarCategoriaPolitica;
        vm.nombrearchivo = "Seleccione Archivo";
        vm.Focalizacion = null;
        vm.PoliticasTransversalesArchivo = [];
        vm.DetalleFocalizacionConsultada = null;
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
        vm.ConvertirNumero4decimales = ConvertirNumero4decimales;
        vm.abrirMensajeInformacionEjecucionPoliticas = abrirMensajeInformacionEjecucionPoliticas;
        vm.abrirMensajeMetaCategoria = abrirMensajeMetaCategoria;
        vm.abrirMensajeMetaIndicador = abrirMensajeMetaIndicador;
        vm.abrirMensajeArchivoFocalizacion = abrirMensajeArchivoFocalizacion;
        vm.abrirMensajeIndicadores = abrirMensajeIndicadores;
        vm.obtenerIndicador = obtenerIndicador;
        vm.obtenerIndicadorAcumulable = obtenerIndicadorAcumulable;
        vm.obtenerUnidadMedidaIndicadorSecundario = obtenerUnidadMedidaIndicadorSecundario;
        vm.obtenerMetaIndicadorSecundario = obtenerMetaIndicadorSecundario;
        vm.obtenerAcumulativoIndicadorSecundario = obtenerAcumulativoIndicadorSecundario;
        vm.longMaxText = 30;
        vm.existeFocalizacion = false;
        vm.PoliticaMostar = 0;
        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.PoliticasTransversalesArchivo = [];
            vm.obtenerPoliticasTransversales();            
            vm.notificarrefresco({ handler: vm.refrescarPoliticas, nombreComponente: vm.nombreComponente });            
        }

        vm.refrescarPoliticas = function () {
            vm.obtenerPoliticasTransversalesCategorias();
        }
        $scope.$watch('vm.tramiteproyectoidcategoria', function () {
            if (vm.tramiteproyectoidcategoria != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoidcategoria) {
                    vm.obtenerPoliticasTransversalesCategorias();
                }
            }
        });
        $scope.$watch('vm.calendariopoliticastransversales', function () {
            if (vm.calendariopoliticastransversales != '') {
                vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;
            }
        });

        vm.obtenerPoliticasTransversales = function () {            
            var idInstancia = $sessionStorage.idInstancia;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;            
            return focalizacionAjustesServicio.obtenerPoliticasTransversalesProyecto($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(                
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {                        
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosGenerales = arregloGeneral.Politicas;
                        var listaPoliticasProy = [];
                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            var habilitarVerDatos = false;
                            if (arregloDatosGenerales[pl].PoliticaId == 4 || arregloDatosGenerales[pl].PoliticaId == 7) {
                                habilitarVerDatos = true;
                            }
                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                habilitarVerDatos: habilitarVerDatos
                            }
                            if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento)
                                listaPoliticasProy.push(politicasProyecto);
                        }
                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                    }
                });
        }
        vm.obtenerPoliticasTransversalesCategorias = function () {
            var idInstancia = $sessionStorage.idNivel;
            const proyecto = {
                InstanciaId: $sessionStorage.idInstancia,
                TramiteId: $sessionStorage.InstanciaSeleccionada.tramiteId,
                ProyectoId: vm.proyectoidcategoria,
                PoliticaId: 0,
                Bpin: vm.BPIN,
                Texto: 'ConsultaCategorias',
                DetalleLocalizacion: []
            };
            var parametroConsulta = JSON.stringify(proyecto);
            return modificacionLeyServicio.ConsultarPoliticasTransversalesCategoriasModificaciones(parametroConsulta).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.listaPoliticasCategorias = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.Focalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.existeFocalizacion = true;                        
                        ValidarVerDescarDocumentos();
                    }
                    else {
                        vm.existeFocalizacion = false;
                    }
                }
            );
        }

        function abrirMensajeArchivoFocalizacion() {
            utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Plantilla Carga Masiva Recursos Categoría modificaciones de Ley y Decreto, </span><br /> <span class='tituhori'><ul><li>No se permite texto en la columna de 'RecursosCartaNacion' y 'RecursosCartaPropios'</li><li>La columna 'RecursosCartaNacion' y 'RecursosCartaPropios' acepta valores numéricos sin separador de mil y dos decimales con separador coma(,)</li><li>El nombre del archivo no debe contener tíldes ni caracteres especiales</li></ul></span>");
        }
        function abrirMensajeIndicadores() {
            utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Seleccionar Indicadores de política de Construcción de Paz para modificaciones de Ley y Decreto, </span><br /> <span class='tituhori'><ul><li>Seleccione el indicador que se debe asociar a la categoría de la política de construcción de Paz.</li></ul></span>");
        }

        vm.AbrilNivel1 = function (fuenteId, indexPoliticas, indexCategorias) {
            var variable = $("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
            variable = variable.replace(/ /g, "");
            variable = variable.replace("\r", "");
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
        };
        vm.AbrilNivel2 = function (fuenteId, productoId, indexPoliticas, indexCategorias, categoriaId, politicaId) {
            var variable = $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias);
            var imgmenos = document.getElementById("imgmenos" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias);
            if (variable === "+") {
                $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

                if (vm.imgmasProdAnt != undefined && vm.imgmenosProdAnt != undefined) {
                    if (vm.imgmasProdAnt !== imgmas) {
                        $("#ico" + vm.FuenteConsultada + "-" + vm.ProductoConsultado + "-" + vm.indexPoliticasConsultada + "-" + vm.indexCategoriasConsultada).html('+');
                        vm.imgmasProdAnt.style.display = 'block';
                        vm.imgmenosProdAnt.style.display = 'none';
                    }
                }


            } else {
                $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.imgmasProdAnt = imgmas;
            vm.imgmenosProdAnt = imgmenos;
            vm.ProductoConsultado = productoId;
            vm.FuenteConsultada = fuenteId;
            vm.CategoriaConsultada = categoriaId;
            vm.PoliticaConsultada = politicaId;
            vm.indexPoliticasConsultada = indexPoliticas;
            vm.indexCategoriasConsultada = indexCategorias;
        };
        vm.AbrilNivel3 = function (productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId) {
            var imgmas = document.getElementById("imgmas-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
            var variableControl = $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
            if (vm.imgmasAnt != undefined && vm.imgmenosAnt != undefined) {
                if (vm.imgmasAnt !== imgmas) {
                    if (variableControl === "+") {
                        if (vm.listaDetalleLocalizacion.HabilitaEditarLocalizador == true) {
                            vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
                            $("#Guardar" + vm.localizacionConsultada + vm.indexPoliticasConsultada + vm.indexCategoriasConsultada + vm.indexFuentesConsultada + vm.indexProductoConsultado).attr('disabled', true);
                            desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);

                        }
                        else {
                            desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
                        }
                    }
                }
                else {
                    desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
                }
            }
            else {
                desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
            }
        };

        function desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId) {

            var recargar = 0;
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
                    if (detail[0] != undefined) {
                        detail[0].classList.remove("hidden");
                    }
                    $("#detail-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).removeClass("hidden");
                }

                if (vm.imgmasAnt != undefined && vm.imgmenosAnt != undefined) {
                    if (vm.imgmasAnt !== imgmas) {
                        recargar = 1;
                        $("#ico" + vm.ProductoConsultado + "-" + vm.FuenteConsultada + "-" + vm.localizacionConsultada + "-" + vm.indexPoliticasConsultada + "-" + vm.indexCategoriasConsultada).html('+');
                        vm.imgmasAnt.style.display = 'block';
                        vm.imgmenosAnt.style.display = 'none';
                        if (vm.detailAnt != undefined) {
                            if (vm.detailAnt[0] != undefined) {
                                vm.detailAnt[0].classList.add("hidden");
                            }
                        }
                    }
                }


                vm.imgmasAnt = imgmas;
                vm.imgmenosAnt = imgmenos;
                vm.detailAnt = detail;
                vm.ProductoConsultado = productoId;
                vm.FuenteConsultada = fuenteId;
                vm.localizacionConsultada = localizacionId;
                vm.CategoriaConsultada = categoriaId;
                vm.indexPoliticasConsultada = indexPoliticas;
                vm.indexCategoriasConsultada = indexCategorias;
                vm.indexFuentesConsultada = indexFuentes;
                vm.indexProductoConsultado = indexProducto;
                vm.PoliticaConsultada = politicaId;
                vm.consultarDetalleLocalizacion(productoId, fuenteId, localizacionId, categoriaId, recargar);

            } else {
                $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                if (detail != undefined) {
                    if (detail[0] != undefined) {
                        detail[0].classList.add("hidden");
                    }
                }
            }
        }       
        vm.volver = function () {
            vm.PoliticaMostar = 0;
        }

        vm.VerPolitica = function (idPolitica) {        
            vm.PoliticaMostar = idPolitica;
        }
        vm.TieneCategorias = function (itemPoliticas) {
            if (itemPoliticas.PoliticaId == 10) {
                console.log("10");
            }
            let tiene = itemPoliticas.Categorias != null && itemPoliticas.Categorias.length > 0 && itemPoliticas.Categorias[0].DimensionId > 0;

            return tiene;
        }
        vm.EditarPolitica = function (politicasIndicadores) {
            if (politicasIndicadores.Editar == true) {
                politicasIndicadores.Editar = false;
                politicasIndicadores.Categorias = JSON.parse(politicasIndicadores.CategoriasCopia);
            }
            else {
                politicasIndicadores.Editar = true;
                politicasIndicadores.CategoriasCopia = JSON.stringify(politicasIndicadores.Categorias);
            }
            $sessionStorage.GuardarAprobacionEntidadVFExec = true;
            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
        }
        vm.GuardarPolitica = function (politicasIndicadores) {
            vm.DatosDimension = [];
            angular.forEach(politicasIndicadores.Categorias, function (categoria) {
                let dimension = {};
                dimension.DimensionId = categoria.DimensionId;
                dimension.DatosDistribucion = [];
                angular.forEach(categoria.Localizaciones, function (localizacion) {
                    let datosDistribucion = {};
                    datosDistribucion.LocalizacionId = categoria.DimensionId;
                    datosDistribucion.ValorDistribuidoNacion = localizacion.ValorRecursosAprobadosNacion;
                    datosDistribucion.ValorDistribuidoPropios = localizacion.ValorRecursosAprobadosPropios;
                    dimension.DatosDistribucion.push(datosDistribucion);
                });
                vm.DatosDimension.push(dimension);
            });
            const proyecto = {  
                TramiteId: $sessionStorage.InstanciaSeleccionada.tramiteId,
                ProyectoId: vm.proyectoidcategoria,
                PoliticaId: politicasIndicadores.PoliticaId,
                DatosDimension: vm.DatosDimension
            };
            return modificacionLeyServicio.GuardarPoliticasTransversalesCategoriasModificaciones(proyecto)
                .then(function (respuesta) {
                    let exito = respuesta.data;
                    if (exito.Exito && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        utilidades.mensajeSuccess('Verifique que todos los datos de las categorías hayan sido diligenciados',
                           false,false,false,'Los datos han sido guardados con éxito');
                        vm.init();
                        vm.obtenerPoliticasTransversalesCategorias();
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        var mensaje = respuesta.data.Mensaje;
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
        vm.EliminarCategoria = function (politicasIndicadores, dimensionId) {
            const proyecto = {
                TramiteId: $sessionStorage.InstanciaSeleccionada.tramiteId,
                ProyectoId: vm.proyectoidcategoria,
                PoliticaId: politicasIndicadores.PoliticaId,
                DimensionId: dimensionId
            };
            utilidades.mensajeWarning('¿Está seguro de continuar?', function funcionContinuar() {
                return comunesServicio.eliminarCategoriasProyectoProgramacion(proyecto)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito.Exito && (response.statusText === "OK" || response.status === 200)) {
                            utilidades.mensajeSuccess(
                                '',false,false,false,'Los datos fueron eliminados con éxito.');
                            vm.init();
                            vm.obtenerPoliticasTransversalesCategorias();
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
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
            }, function funcionCancelar(reason) {
            }, null, null, 'La categoría seleccionada va a ser eliminada de la tabla');
        }
        vm.CalcularValorRecursosPoaiNacion = function (localizaciones) {
            let total = 0;
            angular.forEach(localizaciones, function (item) {
                total += parseFloat(item.ValorDistribuidoNacion);
            });
            return total;
        }
        vm.CalcularValorRecursosCartaNacion = function (localizaciones) {
            let total = 0;
            angular.forEach(localizaciones, function (item) {
                total += parseFloat(item.ValorRecursosAprobadosNacion);
            });
            return total;
        }
        vm.CalcularValorRecursosPoaiPropios = function (localizaciones) {
            let total = 0;
            angular.forEach(localizaciones, function (item) {
                total += parseFloat(item.ValorDistribuidoPropios);
            });
            return total;
        }
        vm.CalcularValorRecursosCartaPropios = function (localizaciones) {
            let total = 0;
            angular.forEach(localizaciones, function (item) {
                total += parseFloat(item.ValorRecursosAprobadosPropios);
            });
            return total;
        }
      
        vm.VerPoliticaIndicadores = function (PoliticaId) {
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
        };

        vm.verNombresCompleto = function (idElement, idElementObj, idElementProd, idElementIppal, idElementIsec, indexElementObj, indexElementProd, indexElementIppal, indexElementIsec, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
            var elValidacion = document.getElementById(idElement + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-val');
            var elCortoObj = document.getElementById(idElementObj + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');
            var elCortoProd = document.getElementById(idElementProd + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');
            var elCortoIppal = document.getElementById(idElementIppal + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');
            var elCortoIsec = document.getElementById(idElementIsec + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');

            var elCompletoObj = document.getElementById(idElementObj + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');
            var elCompletoProd = document.getElementById(idElementProd + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');
            var elCompletoIppal = document.getElementById(idElementIppal + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');
            var elCompletoIsec = document.getElementById(idElementIsec + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');

            if (elCompletoObj.classList.contains('hidden')) {
                elValidacion.innerHTML = 'VER MENOS';
                elCortoObj.classList.add('hidden');
                elCompletoObj.classList.remove('hidden');

                elCortoProd.classList.add('hidden');
                elCompletoProd.classList.remove('hidden');

                elCortoIppal.classList.add('hidden');
                elCompletoIppal.classList.remove('hidden');

                elCortoIsec.classList.add('hidden');
                elCompletoIsec.classList.remove('hidden');

            } else {
                elValidacion.innerHTML = 'VER MÁS';
                elCortoObj.classList.remove('hidden');
                elCompletoObj.classList.add('hidden');

                elCortoProd.classList.remove('hidden');
                elCompletoProd.classList.add('hidden');

                elCortoIppal.classList.remove('hidden');
                elCompletoIppal.classList.add('hidden');

                elCortoIsec.classList.remove('hidden');
                elCompletoIsec.classList.add('hidden');

            }
        };
            
        vm.ConsultarPoliticasCategoriasIndicadores = function (bpin) {
            var idInstancia = $sessionStorage.idInstancia;
            return focalizacionAjustesServicio.ConsultarPoliticasCategoriasIndicadores(vm.BPIN, usuarioDNP, idInstancia).then(
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

        function abrirModalAgregarIndicador(CategoiaId1, PoliticaId) {
            var CategoriaSelec = CategoiaId1;
            $sessionStorage.CategoriaSelec = CategoriaSelec;
            $sessionStorage.PoliticaSelecId = PoliticaId;
            $sessionStorage.ProyectoSelecId = vm.proyectoidcategoria;
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorMl.html',
                controller: 'modalAgregarIndicadorMlController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia"
            }).result.then(function (result) {
                init();
                vm.obtenerPoliticasTransversalesCategorias();
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
            }, function (reason) {
                init();                
                vm.obtenerPoliticasTransversalesCategorias();
            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
        }

        vm.eliminarIndicador = function (IndicadorId, DimensionId, FocalizacionId) {
            var ArregloIndicadores = [{
                IndicadorId: IndicadorId,
                Indicador: null,
                ProyectoId: vm.proyectoId,
                CategoriaId: DimensionId,
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
                focalizacionAjustesServicio.ModificarPoliticasCategoriasIndicadores(indicadoresCategoriasGuardar, usuarioDNP, vm.idInstancia).then(function (response) {
                    if ((response.statusText === "OK" || response.status === 200) && response.data) {
                        var respuestaExito = JSON.parse(response.data.toString()).Exito;
                        var respuestaMensaje = JSON.parse(response.data.toString()).Mensaje;
                        if (respuestaExito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("El indicador fue eliminado con éxito!", false, false, "El indicador fue eliminado con éxito!");
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            init();
                            vm.obtenerPoliticasTransversalesCategorias();

                        } else {
                            swal('', respuestaMensaje, 'error');
                            init();
                            vm.obtenerPoliticasTransversalesCategorias();

                        }
                    }
                });
            }, function funcionCancelar(reason) {

            }, null, null, "El indicador asociado sera eliminado");

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

        vm.habilitarEditar = function (producto, fuente, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto, localizacionDetalle) {
            localizacion.HabilitaEditarLocalizador = true;
            vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = true;
            $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).attr('disabled', false);
            $("#ValoresCero" + producto + fuente + localizacion.LocalizacionId).attr('disabled', false);
            angular.forEach(localizacionDetalle.FocalizacionAjustada, function (series) {
                series.EnAjusteOriginal = series.EnAjuste;
                series.EnAjusteFOriginal = series.EnAjusteF;
                series.MetaCategoriaOriginal = series.MetaCategoria;
                series.MetaCategoriaFOriginal = series.MetaCategoriaF;
                series.PersonasCategoriaOriginal = series.PersonasCategoria;
                series.PersonasCategoriaFOriginal = series.PersonasCategoriaF;
                series.MetaIndicadorSecundarioOriginal = series.MetaIndicadorSecundario;
                series.MetaIndicadorSecundarioFOriginal = series.MetaIndicadorSecundarioF;
            });
        }

        vm.cancelarEdicion = function (politica, categoria, fuente, producto, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
            var idInstancia = $sessionStorage.idNivel;
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                localizacion.HabilitaEditarLocalizador = false;
                vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
                $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).attr('disabled', true);
                $("#ValoresCero" + producto + fuente + localizacion.LocalizacionId).attr('disabled', true);
                asignarValoresOriginales(politica, categoria, fuente, producto, localizacion);
            }, function funcionCancelar(reason) {
            }, 'Aceptar', 'Cancelar', 'Los posibles datos que haya diligenciado en la tabla "Focalización ajustada" se perderán.');

        }

        vm.actualizaFila = function (localizacion) {
            calcularTotales(localizacion);
        }

        vm.mostrarBotones = function (origen, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
            switch (origen) {
                case 1:
                    {
                        $("#Editar" + localizacion.LocalizacionId + '-' + indexPoliticas + '-' + indexCategorias + '-' + indexFuentes + '-' + indexProducto).attr('disabled', false);
                        $("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).fadeIn();
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
            localizacion = vm.listaDetalleLocalizacion;
            angular.forEach(localizacion.FocalizacionAjustada, function (series) {
                series.EnAjuste = series.EnAjusteOriginal;
                series.EnAjusteF = series.EnAjusteFOriginal;
                series.MetaCategoria = series.MetaCategoriaOriginal;
                series.MetaCategoriaF = series.MetaCategoriaFOriginal;
                series.PersonasCategoria = series.PersonasCategoriaOriginal;
                series.PersonasCategoriaF = series.PersonasCategoriaFOriginal;
                series.MetaIndicadorSecundario = series.MetaIndicadorSecundarioOriginal;
                series.MetaIndicadorSecundarioF = series.MetaIndicadorSecundarioFOriginal;
            });
            localizacion.HabilitaEditarLocalizador = false;
            vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
            calcularTotales(localizacion);
        }

        function calcularTotales(localizacion) {
            var valorTotalEnAjuste = 0;
            var valorTotalMetaCategoria = 0;
            var valorTotalPersonasCategoria = 0;
            var valorTotalMetaIndicadorSecundario = 0;
            localizacion = vm.listaDetalleLocalizacion;
            for (var laj = 0; laj < localizacion.FocalizacionAjustada.length; laj++) {
                valorTotalEnAjuste += localizacion.FocalizacionAjustada[laj].EnAjuste == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].EnAjuste);
                valorTotalMetaCategoria += localizacion.FocalizacionAjustada[laj].MetaCategoria == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].MetaCategoria);
                valorTotalPersonasCategoria += localizacion.FocalizacionAjustada[laj].PersonasCategoria == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].PersonasCategoria);
                valorTotalMetaIndicadorSecundario += localizacion.FocalizacionAjustada[laj].MetaIndicadorSecundario == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].MetaIndicadorSecundario);
            }
            localizacion.valorTotalEnAjuste = vm.ConvertirNumero2decimales(valorTotalEnAjuste);
            localizacion.valorTotalMetaCategoria = valorTotalMetaCategoria;
            localizacion.valorTotalPersonasCategoria = valorTotalPersonasCategoria;
            localizacion.valorTotalMetaIndicadorSecundario = valorTotalMetaIndicadorSecundario;

            vm.listaDetalleLocalizacion.valorTotalEnAjuste = localizacion.valorTotalEnAjuste;
            vm.listaDetalleLocalizacion.valorTotalMetaCategoria = localizacion.valorTotalMetaCategoria;
            vm.listaDetalleLocalizacion.valorTotalPersonasCategoria = localizacion.valorTotalPersonasCategoria;
            vm.listaDetalleLocalizacion.valorTotalMetaIndicadorSecundario = localizacion.valorTotalMetaIndicadorSecundario;
        }

        function calcularTotalesGeneral() {
            var TotalFocalizacoOriginalProducto = 0;
            var TotalFocalizacoProducto = 0;
            var TotalFocalizadoFuente = 0;
            var TotalFocalizadoCategoria = 0;            
        }

        function actualizarIndicadoresPolitica() {
            var TotalFocalizado = 0;
            if (vm.listaPoliticasCategoriasIndicadores.Politicas != null) {
                vm.listaPoliticasCategoriasIndicadores.Politicas.forEach(politicas => {
                    if (politicas.PoliticaId == vm.PoliticaConsultada) {
                        if (politicas.Categorias != null) {
                            politicas.Categorias.forEach(categorias => {
                                if (categorias.DimensionId == vm.CategoriaConsultada) {
                                    categorias.EstaFocalizada = 0;
                                    if (vm.CategoriaConsultadaFocalizada === 1) {
                                        categorias.EstaFocalizada = 1;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
        
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
                return focalizacionAjustesServicio.eliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId, usuarioDNP, idInstancia).then(
                    function (respuesta) {
                        let exito = respuesta.data;
                        if (exito.Exito && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            guardarCapituloModificado();
                            new utilidades.mensajeSuccess("", false, false, false, mensaje3);
                            vm.init();
                            vm.obtenerPoliticasTransversalesCategorias();
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        } else {
                            new utilidades.mensajeError("No se puede eliminar la politica seleccionada, se encuentra con valores en firme.");
                        }
                    });
            }, function funcionCancelar(reason) {
            }, null, null, 'Los datos serán eliminados.');
        }

        function ValidarVerDescarDocumentos() {
            vm.BloquearCargue = false;
            vm.Focalizacion.Politicas.forEach(politicas => {
                if (politicas.Categorias == null)
                    vm.existeFocalizacion = false;
                if (politicas.TieneConceptoPendiente == true) {
                    vm.BloquearCargue = true;
                }
            });
        }

        function abrirMensajeInformacionEjecucionPoliticas() {
            utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Las categorías sobre las que ya registró información sobre su ejecución no se encuentran disponibles para ser eliminadas.</span>");
        }

        function abrirMensajeMetaCategoria() {
            utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <br /> <br /> <span class='tituhori' > El valor del campo ''Meta categoría'' debe ser menor o igual a la meta del indicador principal del producto por vigencia.</span>");
        }

        function abrirMensajeMetaIndicador() {
            utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > El valor del campo ''Meta indicador secundario'' debe ser menor o igual a la meta del indicador secundario del producto que tiene la marca de equidad de la mujer, por vigencia.</span>");
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
                vm.init();
                vm.obtenerPoliticasTransversalesCategorias();
            }
        }
        vm.disabled = function (habilitaVerMas) {
            if (habilitaVerMas) {
                return false;
            } else {
                return true;
            }
        }
        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }
        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }        

        vm.getNombreReducido = function (data, maxCaracteres) {
            if (data.length > maxCaracteres) {
                var dataNueva = data.slice(0, maxCaracteres);
                return dataNueva + '...';
            } else return data
        }
        function abrirModalAgregarCategoriaPolitica(Politica) {
            $sessionStorage.InstanciaSeleccionada.IdEntidad = $sessionStorage.InstanciaSeleccionada.entidadId;
            $sessionStorage.idProyectoEncabezado = vm.proyectoidcategoria;
            var categoriasXPolitica = [];
            for (var lpc = 0; lpc < vm.listaPoliticasCategorias.PoliticasCategorias.length; lpc++) {
                if (vm.listaPoliticasCategorias.PoliticasCategorias[lpc].Categorias != null && vm.listaPoliticasCategorias.PoliticasCategorias[lpc].PoliticaId === Politica.PoliticaId) {
                    for (var cats = 0; cats < vm.listaPoliticasCategorias.PoliticasCategorias[lpc].Categorias.length; cats++) {
                        categoriasXPolitica.push(vm.listaPoliticasCategorias.PoliticasCategorias[lpc].Categorias[cats].DimensionId);
                    }
                }
            }
            $sessionStorage.categoriasXPolitica = categoriasXPolitica;
            var data = {
                idpolitica: Politica.PoliticaId,
                nombrePoliticaCat: Politica.NombrePolitica
            }
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: "src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/FocalizacionCategoria/modalAgregarCategoriaPoliticaMl.html",
                controller: 'modalAgregarCategoriaPoliticaMlController',
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
                    init();
                    vm.obtenerPoliticasTransversalesCategorias();
                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                }
            });
        }
        /*Carga Masiva*/
        vm.activarControles = function (evento, politicaId) {
            switch (evento) {
                case "inicio":
                    $("#btnPoliticasTransversalesValidarArchivo-" + politicaId).attr('disabled', true);
                    $("#btnPoliticasTransversalesLimpiarArchivo-" + politicaId).attr('disabled', true);
                    $("#btnPoliticasTransversalesArchivoSeleccionado-" + politicaId).attr('disabled', true);
                    document.getElementById("politicastransversales-" + politicaId).value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnPoliticasTransversalesValidarArchivo-" + politicaId).attr('disabled', false);
                    $("#btnPoliticasTransversalesLimpiarArchivo-" + politicaId).attr('disabled', false);
                    $("#btnPoliticasTransversalesArchivoSeleccionado-" + politicaId).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnPoliticasTransversalesValidarArchivo-" + politicaId).attr('disabled', false);
                    $("#btnPoliticasTransversalesLimpiarArchivo-" + politicaId).attr('disabled', false);
                    $("#btnPoliticasTransversalesArchivoSeleccionado-" + politicaId).attr('disabled', false);
                    break;
                default:
            }
        }

        function HabilitaEditar(band) {
            vm.HabilitaEditarBandera = band;
        }

        function CargarArchivoPoliticasTransversales(politicasIndicadores) {
            if (vm.PoliticasTransversalesArchivo.length > 0) {
                vm.DatosDimension = [];
                angular.forEach(politicasIndicadores.Categorias, function (categoria) {
                    let dimension = {};
                    dimension.DimensionId = categoria.DimensionId;
                    dimension.DatosDistribucion = [];
                    let agregar = false;
                    angular.forEach(categoria.Localizaciones, function (localizacion) {
                        let datosDistribucion = {};
                        datosDistribucion.LocalizacionId = categoria.DimensionId;
                        angular.forEach(vm.PoliticasTransversalesArchivo, function (fila) {
                            if (fila.DimensionId == categoria.DimensionId) {
                                datosDistribucion.ValorDistribuidoNacion = fila.ValorDistribuidoNacion;
                                datosDistribucion.ValorDistribuidoPropios = fila.ValorDistribuidoPropios;
                                dimension.DatosDistribucion.push(datosDistribucion);
                                agregar = true;
                            }
                        });
                    });
                    if (agregar) {
                        vm.DatosDimension.push(dimension);
                    }
                });
                const proyecto = {
                    TramiteId: $sessionStorage.InstanciaSeleccionada.tramiteId,
                    ProyectoId: vm.proyectoidcategoria,
                    PoliticaId: politicasIndicadores.PoliticaId,
                    DatosDimension: vm.DatosDimension
                };
                return modificacionLeyServicio.GuardarPoliticasTransversalesCategoriasModificaciones(proyecto)
                    .then(function (respuesta) {
                        let exito = respuesta.data;
                        if (exito.Exito && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            utilidades.mensajeSuccess(
                                'Verifique que todos los datos de las categorías hayan sido diligenciados',
                                false,false,false,'Los datos han sido guardados con éxito');
                            vm.obtenerPoliticasTransversalesCategorias();
                            vm.activarControles('inicio', politicasIndicadores.PoliticaId);
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        }
                        else {
                            var mensaje = respuesta.data.Mensaje;
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
        }

        function exportarFocalizacionExcel(politicasIndicadores) {
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
                    {name: 'TramiteId', title: 'Tramite Id'
                    },
                    {name: 'ProyectoId', title: 'Proyecto Id'
                    },
                    {name: 'PoliticaId', title: 'Politica Id'
                    },
                    {name: 'DimensionId', title: 'Dimension Id'
                    },                    
                    {name: 'Dimension', title: 'Dimension'
                    },                    
                    {name: 'RecursosCartaNacion', title: 'Recursos Carta Nacion'
                    },
                    {name: 'RecursosCartaPropios', title: 'Recursos Carta Propios'
                    }
                ];
                let colNames = columns.map(function (item) {
                    return item.title;
                })
                var wb = XLSX.utils.book_new();
                wb.Props = {
                    Title: "Plantilla Recursos",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };
                wb.SheetNames.push("Recursos");
                const header = colNames;
                const data = [];
                var NombrePolitica = "";
                politicasIndicadores.Categorias.forEach(categoria => {
                    categoria.Localizaciones.forEach(localizacion => {
                        data.push({
                            TramiteId: $sessionStorage.InstanciaSeleccionada.tramiteId,
                            ProyectoId: vm.proyectoidcategoria,
                            PoliticaId: politicasIndicadores.PoliticaId,
                            DimensionId: categoria.DimensionId,                            
                            Dimension: categoria.NombreCategoria + ' - ' + categoria.NombreSubcategoria,
                            RecursosCartaNacion: localizacion.ValorRecursosAprobadosNacion,
                            RecursosCartaPropios: localizacion.ValorRecursosAprobadosPropios
                        });
                    });
                });

                NombrePolitica = NombrePolitica.replace('á', 'a')
                NombrePolitica = NombrePolitica.replace('é', 'e')
                NombrePolitica = NombrePolitica.replace('í', 'i')
                NombrePolitica = NombrePolitica.replace('ó', 'o')
                NombrePolitica = NombrePolitica.replace('ú', 'u')

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: ["TramiteId",
                        "ProyectoId",
                        "PoliticaId",
                        "DimensionId",
                        "Dimension",
                        "RecursosCartaNacion",
                        "RecursosCartaPropios"]
                });
                for (let col of [5]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [6]) {
                    formatColumn(worksheet, col, "#,##")
                }                
                worksheet['!cols'] = [];
                worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };                

                wb.Sheets["Recursos"] = worksheet;
                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });                
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaRecursos' + NombrePolitica + '.xlsx');
            }, function funcionCancelar(reason) {
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
        function adjuntarArchivo(politicaId) {
            document.getElementById('politicastransversales-' + politicaId).value = "";
            document.getElementById('politicastransversales-' + politicaId).click();
        }
        $scope.politicastransversalesNameChanged = function (input) {
            let idPolitica = input.id.toString().substring(input.id.indexOf('-') + 1, input.id.length);
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                vm.activarControles('cargaarchivo', idPolitica);
            }
            else {
                vm.activarControles('inicio', idPolitica);
            }
        }        
        function limpiarArchivo(politicaId) {
            $scope.filesfocalizacion = [];
            document.getElementById('politicastransversales-' + politicaId).value = "";
            vm.activarControles('inicio', politicaId);
        }
    
        function validarArchivo(politicaId) {
            var resultado = true;
            vm.PoliticasTransversalesArchivo = [];
            if (document.getElementById('politicastransversales-' + politicaId).files.length > 0) {

                let file = document.getElementById("politicastransversales-" + politicaId).files[0];
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

                                    if (item["TramiteId"] == undefined) {
                                        utilidades.mensajeError("La columna TramiteId no trae valor!");
                                        return false;
                                    }

                                    if (item["ProyectoId"] == undefined) {
                                        utilidades.mensajeError("La columna ProyectoId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["ProyectoId"])) {
                                        utilidades.mensajeError("El valor ProyectoId " + item["ProyectoId"] + " no es númerico!");
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
                                    if (item["DimensionId"] == undefined) {
                                        utilidades.mensajeError("La columna DimensionId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["DimensionId"])) {
                                        utilidades.mensajeError("El valor DimensionId " + item["DimensionId"] + " no es númerico!");
                                        return false;
                                    }                                    
                                    if (item["RecursosCartaNacion"] == undefined) {
                                        utilidades.mensajeError("La columna Carta Nacion no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["RecursosCartaNacion"].toString(), 2)) {
                                        utilidades.mensajeError("El valor Carta Nacion " + item["RecursosCartaNacion"] + " no es númerico!");
                                        return false;
                                    }
                                    if (item["RecursosCartaPropios"] == undefined) {
                                        utilidades.mensajeError("La columna Carta Propios no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["RecursosCartaPropios"].toString(), 2)) {
                                        utilidades.mensajeError("El valor Carta Propios " + item["RecursosCartaPropios"] + " no es númerico!");
                                        return false;
                                    }
                                    var valoresarchivo = {
                                        TramiteId: item["TramiteId"],
                                        ProyectoId: item["ProyectoId"],
                                        PoliticaId: item["PoliticaId"],
                                        DimensionId: item["DimensionId"],
                                        Dimension: item["Dimension"],                                        
                                        ValorDistribuidoNacion: item["RecursosCartaNacion"],
                                        ValorDistribuidoPropios: item["RecursosCartaPropios"]
                                    };

                                    vm.PoliticasTransversalesArchivo.push(valoresarchivo);
                                });
                                if (resultado.indexOf(false) == -1) {
                                    vm.activarControles('validado', politicaId);
                                }
                                else {
                                    vm.activarControles('inicio', politicaId);
                                    vm.PoliticasTransversalesArchivo = [];
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
        }
        function ValidarDicimal(valor, decimals) {
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
            return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
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
        function ejecutarConsultaCargueMasivo(politicasIndicadores) {
            exportarFocalizacionExcel(politicasIndicadores);
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
                Modificado: false,
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
                                        if (categorias.CategoriaId == categoriaId) {
                                            categorias.presentaError2Categoria = true;
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
        }
        vm.errores = {
            'FOC_CAT_001': vm.validarFOC_CAT_001,
            'FOC_CAT_002': vm.validarFOC_CAT_002,
            'FOC_CAT_003': vm.validarFOC_CAT_003,
        }
    }

    angular.module('backbone').component('resumenFocalizacionMlFormulario', {
        templateUrl: "src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/ResumenFocalizacionMlFormulario.html",
        controller: resumenFocalizacionMlFormularioController,
        controllerAs: "vm",
        bindings: {
            tramiteproyectoidcategoria: '@',
            proyectoidcategoria: '@',
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&',
            calendariopoliticastransversales: '@'
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

                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();