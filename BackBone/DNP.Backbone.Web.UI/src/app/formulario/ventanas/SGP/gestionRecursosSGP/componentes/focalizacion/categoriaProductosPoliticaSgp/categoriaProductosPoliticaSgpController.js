(function () {
    'use strict';

    categoriaProductosPoliticaSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'gestionRecursosSGPServicio',
        '$anchorScroll',
        '$location',
        'utilidades',
        '$compile',
        'justificacionCambiosServicio'
    ];

    function categoriaProductosPoliticaSgpController(
        $scope,
        $sessionStorage,
        gestionRecursosSGPServicio,
        $anchorScroll,
        $location,
        utilidades,
        $compile,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.elementoOrigen = $sessionStorage.nombreElementoOrigen;
        $scope.datos = [];
        vm.catProdPolitica = {};
        vm.fuenteId = sessionStorage.getItem("fuenteId");
        vm.politicaId = sessionStorage.getItem("politicaId");
        vm.politicaIdEquidadMujer = 7;
        vm.mensajeSinDatosLoc = '¡Sin datos de localizaciones!';
        vm.nombreElementoOrigen = sessionStorage.getItem("nombreElementoOrigen");
        /*vm.nombreComponente = "sgpsolicitudrecursosfocalizacioncategoriapoliticassgp";*/
        vm.nombreComponente = "sgpsolicitudrecursosfocalizacionpoliticassgp";

        vm.fuenteConsultada = 0;
        vm.politicaConsultada = 0;
        vm.productoConsultado = 0;
        vm.localizacionConsultada = 0;
        vm.soloLectura = false;

        //Inicio
        vm.init = function () {
            vm.cargarCss('/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/categoriaProductosPoliticaSgp/categoriaProductosPoliticaSgp.css',            
                'categoriaProductosPoliticaSgpCss');
            vm.loadJavascript('/Scripts/jquery-ui/jquery.mask.js', 'jquery.mask');
            vm.obtenerDatosCPP(vm.BPIN, vm.fuenteId, vm.politicaId);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        /* ------------------------ lunas ---------------------------------*/

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-sgpsolicitudrecursosfocalizacionpoliticassgp');
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
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

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            vm.init();
        }
        /* -----------------------------------------------------------------*/

        vm.scrollTo = function (id) {
            var old = $location.hash();
            $location.hash(id);
            //$anchorScroll.yOffset = 50;
            $anchorScroll();
            //reset to old to keep any additional routing logic from kicking in
            $location.hash(old);
        };

        vm.retornar = function (event) {
            var nombreElementoOrigen = '';
            var e = event.currentTarget;
            if (e) {
                var att = e.getAttribute("data-attribute");
                if (att) {
                    nombreElementoOrigen = att;
                }
            }

            //var nombreElementoOrigen = sessionStorage.getItem("nombreElementoOrigen");
            if (nombreElementoOrigen)
                vm.scrollTo(nombreElementoOrigen);

            vm.ocultarCatProd(e);
        };

        vm.ocultarCatProd = function (e) {
            var e2 = e.closest(".cDivMainCatProdPol");
            if (e2) {
                var p = e2.parentNode;
                var p2 = p.parentNode.parentNode;

                if (p) {
                    $(p).css("display", "none");
                }

                if (p2) {
                    $(p2).css("display", "none");
                }
            }
        };

        vm.ocultarCatProd = function () {
            $("#contenedorCategoriaProductosPoliticaSgp").css("visibility", "hidden");
        };

        /// Cargar archivo de CSS dinamico.
        vm.cargarCss = function (url, id) {
            var old = document.getElementById(id);
            //eval("var old = document.getElementById('" + id + "')");
            if (old !== null) {
                return;
            }

            var link = document.createElement("link");
            link.id = id;
            link.async = true;
            link.type = "text/css";
            link.rel = "stylesheet";
            link.href = url;
            document.getElementsByTagName("head")[0].appendChild(link);
        };

        /// Cargar archivo de JS dinamico.
        vm.loadJavascript = function (src, id) {
            var old = document.getElementById(id);
            if (old !== null) {
                old.parentNode.removeChild(old);
            }
            var head = document.getElementsByTagName("head")[0];
            var script = document.createElement('script');
            script.id = id;
            //script.async = true;
            script.defer = true;
            script.type = 'text/javascript';
            script.src = src;
            head.appendChild(script);
        };

        vm.activarDescripcionIndicador = function (_this) {
            if (_this) {
                var element = _this.currentTarget;
                if (element) {
                    var iDescripcionIndicador = element;
                    if (iDescripcionIndicador) {
                        iDescripcionIndicador.classList.toggle("fa-plus-square");
                        iDescripcionIndicador.classList.toggle("fa-minus-square");
                    }

                    var dDescripcionInd;
                    var dRepeat = iDescripcionIndicador.parentNode.parentNode;
                    if (dRepeat) {
                        var d = dRepeat.getElementsByClassName('dCPP');
                        if (d && d.length > 0) {
                            dDescripcionInd = d[0];
                        }
                    }

                    var cConectorDiv;
                    var cConectorDivAux = dRepeat.getElementsByClassName('cConectorDiv');
                    if (cConectorDivAux && cConectorDivAux.length > 0) {
                        cConectorDiv = cConectorDivAux[0];
                    }

                    if (iDescripcionIndicador.classList.contains("fa-plus-square")) {
                        if (dDescripcionInd) {
                            dDescripcionInd.style.display = 'none';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'none';
                        }
                    }
                    else if (iDescripcionIndicador.classList.contains("fa-minus-square")) {
                        if (dDescripcionInd) {
                            dDescripcionInd.style.display = 'block';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'block';
                        }
                    }
                }
            }
        };

        vm.recortarTexto = function (texto, longitud) {
            var totalLongitudTexto = texto.length;
            if (texto && totalLongitudTexto > longitud) {
                texto = texto.substring(0, longitud) + '...';
            } else if (texto && totalLongitudTexto > 0) {
                var u = texto.trim().substring(totalLongitudTexto - 1, 1);
                if (u && u !== '.') {
                    texto += '.';
                }
            }

            return texto;
        };

        vm.verMasDescripcionCategoriaPP = function (element, nombre, nombreClase) {
            if (element) {
                $("#dModalCPPContent").html("");
                $("#cPPModalTitle").html("");
                var currentTarget = element.currentTarget;
                if (currentTarget) {
                    var parent = currentTarget.parentNode;
                    if (parent) {
                        var hd = parent.getElementsByClassName(nombreClase);
                        if (hd && hd.length > 0) {
                            var texto = hd[0].value;
                            $("#dModalCPPContent").html(texto);
                            $("#cPPModalTitle").html(nombre);
                        }
                    }
                }
            }
        };

        /// Obtener información de CPP.
        vm.obtenerDatosCPP = function (id, fuenteId, politicaId) {
            if (vm.fuenteId != null && vm.politicaId)
                gestionRecursosSGPServicio
                    .obtenerDatosCPP($sessionStorage.idInstancia, fuenteId, politicaId)
                    .then(
                        (respuesta) => {
                            if (respuesta.data != null && respuesta.data != "") {
                                var jResult = JSON.parse(respuesta.data);
                                if (jResult && typeof jResult === 'string') {
                                    jResult = JSON.parse(jResult);
                                }

                                /// Lista de las fuentes de financiación.
                                var cantidadFuentes = 0;
                                var listaFuenteFinanciacion = sessionStorage.getItem("listaFuenteFinanciacion");
                                var lFuenteFinanciacion;
                                if (listaFuenteFinanciacion) {
                                    lFuenteFinanciacion = jQuery.parseJSON(listaFuenteFinanciacion);
                                    if (lFuenteFinanciacion && lFuenteFinanciacion.Datos_Generales) {
                                        cantidadFuentes = lFuenteFinanciacion.Datos_Generales.length;
                                    }
                                }

                                /// Una sola fuente de financiación.
                                //var datosVigenciaFuente = [];
                                var unicaFuente = (cantidadFuentes === 1);
                                //if (unicaFuente) {
                                //    var q = lFuenteFinanciacion.Datos_Generales.filter(e => e.FuenteId.toString() === fuenteId.toString());
                                //    if (q && q.length > 0) {
                                //        datosVigenciaFuente = q[0].Vigencia;
                                //    }
                                //}

                                if (jResult && jResult.Politicas) {

                                    var n = '';
                                    function formatoNombreLocalizacion(nombre) {
                                        n = nombre.trim();
                                        if (n && n.endsWith('-')) {
                                            n = n.substring(0, n.length - 1);
                                            formatoNombreLocalizacion(n);
                                        }
                                    };

                                    for (var i = 0; i < jResult.Politicas.length; i++) {
                                        var p = jResult.Politicas[i].Politica;
                                        //  jResult.Politicas[i].Politica = vm.capitalizarPrimeraLetra(p);

                                        for (var ij = 0; ij < jResult.Politicas[i].Productos.length; ij++) {
                                            var localizaciones = jResult.Politicas[i].Productos[ij].Localizaciones;
                                            if (localizaciones && localizaciones.length > 0) {
                                                for (var ix = 0; ix < localizaciones.length; ix++) {
                                                    var localizacion = localizaciones[ix].Localizacion;
                                                    if (localizacion) {
                                                        formatoNombreLocalizacion(localizacion);
                                                        localizaciones[ix].Localizacion = n;
                                                    }

                                                    /// Validacion unica fuente.
                                                    /*if (unicaFuente && datosVigenciaFuente.length > 0) {*/
                                                    if (unicaFuente) {
                                                        try {
                                                            for (var ij2 = 0; ij2 < localizaciones[ix].Categorias.length; ij2++) {
                                                                var vigencias = localizaciones[ix].Categorias[ij2].Vigencias;
                                                                if (vigencias) {
                                                                    for (var i1 = 0; i1 < vigencias.length; i1++) {
                                                                        var solicitud = vigencias[i1].SolicitudRecursosCategoria;
                                                                        var costosCategoriaMGA = vigencias[i1].CostosCategoriaMGA;

                                                                        if (solicitud === 0) {
                                                                            vigencias[i1].SolicitudRecursosCategoria = costosCategoriaMGA;
                                                                        }

                                                                        //var vigencias2 = vigencias[i1].Vigencia;
                                                                        //var costosCategoriaMGA = vigencias[i1].CostosCategoriaMGA;
                                                                        //if (vigencias2) {
                                                                        //    vigencias[i1].SolicitudRecursosCategoria = costosCategoriaMGA;
                                                                        //    //var v = datosVigenciaFuente.filter(a => a.Vigencia === vigencias2);
                                                                        //    //if (v && v.length > 0 && solicitud === 0) {
                                                                        //    //    //if (v && v.length > 0) {
                                                                        //    //    vigencias[i1].SolicitudRecursosCategoria = v[0].Valor;
                                                                        //    //}
                                                                        //}
                                                                    }
                                                                }
                                                            }
                                                        } catch (e) { }
                                                    }

                                                    /// Formato datos.
                                                    var categorias = localizaciones[ix].Categorias;
                                                    if (categorias) {
                                                        for (var j11 = 0; j11 < categorias.length; j11++) {
                                                            var categoria = categorias[j11];
                                                            if (categoria && categoria.Vigencias) {
                                                                for (var j12 = 0; j12 < categoria.Vigencias.length; j12++) {
                                                                    var vigencia = categoria.Vigencias[j12];
                                                                    if (vigencia) {
                                                                        //vigencia.SolicitudRecursosCategoria = vm.formatValores(vigencia.SolicitudRecursosCategoria);
                                                                        vigencia.SolicitudRecursosCategoria = vigencia.SolicitudRecursosCategoria;
                                                                    }
                                                                }
                                                            }

                                                            var detalleEtnicos = categorias[j11].DetalleEtnicos;
                                                            if (detalleEtnicos) {
                                                                for (var j13 = 0; j13 < detalleEtnicos.length; j13++) {
                                                                    var dEtnico = detalleEtnicos[j13];
                                                                    if (dEtnico) {
                                                                        dEtnico.PoblacionMGA = vm.formatValores(dEtnico.PoblacionMGA);
                                                                        dEtnico.CostosMGA = vm.formatValores(dEtnico.CostosMGA);
                                                                        dEtnico.SolicitudRecurso = vm.formatValores(dEtnico.SolicitudRecurso);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            /// Indicadores
                                            var indicadores = jResult.Politicas[i].Productos[ij].Indicadores;
                                            if (indicadores && indicadores.length > 0) {
                                                for (var j1 = 0; j1 < indicadores.length; j1++) {
                                                    var indicador = indicadores[j1];
                                                    if (indicador) {
                                                        indicador.Costo = vm.formatValores(indicador.Costo);
                                                    }
                                                }
                                            }
                                            var nombreProducto = jResult.Politicas[i].Productos[ij].NombreProducto;
                                            if (nombreProducto) {
                                                var e = nombreProducto.split('.');
                                                if (e) {
                                                    var p1 = e[0];
                                                    var p2 = e[1];
                                                    var r = p1 + p2;
                                                    jResult.Politicas[i].Productos[ij].orden = parseInt(r);
                                                }
                                            }
                                        }

                                        jResult.Politicas[i].Productos.sort((a, b) => (a.orden > b.orden) ? 1 : -1);

                                    }

                                    vm.catProdPolitica = jResult;
                                    $scope.datos = jResult.Politicas;
                                    vm.soloLectura = $sessionStorage.soloLectura;
                                }
                            } else {
                                vm.imprimirMensaje('¡Sin datos de consulta!.');
                            }
                        });
        };

        vm.formatValores = function (valor) {
            try {
                if (valor) {
                    valor = parseFloat(valor.toString().replace(",", "."))
                    valor = new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(valor);
                }
            } catch (e) {
                console.log(e);
            }

            return valor;
        };

        vm.formatoValores = function (valor, decimales = 2) {
            try {
                if (valor) {
                    valor = (valor.toString().indexOf('.') >= 0 || valor.toString().indexOf(',') >= 0) ? valor.toString().replace(/\./g, "").replace(/\,/g, ".") : valor;
                    valor = new Intl.NumberFormat('es-co', { minimumFractionDigits: decimales, }).format(valor);
                }
            } catch (e) {
                console.log(e);
            }

            return valor;
        };

        vm.activarDescripcion = function (_this, nombreDivMuestra, conector) {
            if (_this) {
                var element = _this.currentTarget;
                if (element) {
                    var iDescripcionIndicador = element;
                    var iconPlus = 'iconPlusTercerNivel'; //'fa-plus-square';
                    var iconMinus = 'iconMinusTercerNivel'; //'fa-minus-square';
                    if (iDescripcionIndicador) {
                        iDescripcionIndicador.classList.toggle(iconPlus);
                        iDescripcionIndicador.classList.toggle(iconMinus);
                    }

                    var dDescripcionInd;
                    var dRepeat = iDescripcionIndicador.parentNode.parentNode;
                    if (dRepeat) {
                        var d = dRepeat.getElementsByClassName(nombreDivMuestra);
                        if (d && d.length > 0) {
                            dDescripcionInd = d[0];
                        }
                    }

                    var cConectorDiv;
                    var cConectorDivAux = dRepeat.getElementsByClassName(conector);
                    if (cConectorDivAux && cConectorDivAux.length > 0) {
                        cConectorDiv = cConectorDivAux[0];
                    }

                    if (iDescripcionIndicador.classList.contains(iconPlus)) {
                        if (dDescripcionInd) {
                            dDescripcionInd.style.display = 'none';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'none';
                        }
                    }
                    else if (iDescripcionIndicador.classList.contains(iconMinus)) {
                        if (dDescripcionInd) {
                            dDescripcionInd.style.display = 'block';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'block';
                        }
                    }
                }
            }
        };

        vm.activarDescripcionLocalizacion = function (_this, nombreDivMuestra, conector, fuenteId, politicaId, productoId, localizacionId) {
            if (_this) {
                var element = _this.currentTarget;
                if (element) {
                    var iDescripcionIndicador = element;
                    var iconPlus = 'iconPlusSegundoNivel'; //'fa-plus-square';
                    var iconMinus = 'iconMinusSegundoNivel'; //'fa-minus-square';
                    if (iDescripcionIndicador) {
                        iDescripcionIndicador.classList.toggle(iconPlus);
                        iDescripcionIndicador.classList.toggle(iconMinus);
                    }

                    var dDescripcionInd;
                    var dRepeat = iDescripcionIndicador.parentNode.parentNode;
                    if (dRepeat) {
                        var d = dRepeat.getElementsByClassName(nombreDivMuestra);
                        if (d && d.length > 0) {
                            dDescripcionInd = d[0];
                        }
                    }

                    var cConectorDiv;
                    var cConectorDivAux = dRepeat.getElementsByClassName(conector);
                    if (cConectorDivAux && cConectorDivAux.length > 0) {
                        cConectorDiv = cConectorDivAux[0];
                    }

                    if (iDescripcionIndicador.classList.contains(iconPlus)) {
                        if (dDescripcionInd) {
                            dDescripcionInd.style.display = 'none';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'none';
                        }
                    }
                    else if (iDescripcionIndicador.classList.contains(iconMinus)) {
                        if (dDescripcionInd) {
                            dDescripcionInd.style.display = 'block';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'block';
                        }
                    }
                }
            }

            if (vm.fuenteConsultada != 0 && vm.productoConsultado != 0 && vm.localizacionConsultada != 0) {
                if (vm.fuenteConsultada != fuenteId && vm.politicaConsultada != politicaId && vm.productoConsultado != productoId && vm.localizacionConsultada != localizacionId) {
                    if (_this) {
                        var element = _this.currentTarget;
                        if (element) {
                            var iDescripcionIndicador = element;
                            var iconPlus = 'iconPlusSegundoNivel'; //'fa-plus-square';
                            var iconMinus = 'iconMinusSegundoNivel'; //'fa-minus-square';
                            if (iDescripcionIndicador) {
                                iDescripcionIndicador.classList.toggle(iconPlus);
                                iDescripcionIndicador.classList.toggle(iconMinus);
                            }

                            var dDescripcionInd;
                            var dRepeat = iDescripcionIndicador.parentNode.parentNode;
                            if (dRepeat) {
                                var d = dRepeat.getElementsByClassName(nombreDivMuestra);
                                if (d && d.length > 0) {
                                    dDescripcionInd = d[0];
                                }
                            }

                            var cConectorDiv;
                            var cConectorDivAux = dRepeat.getElementsByClassName(conector);
                            if (cConectorDivAux && cConectorDivAux.length > 0) {
                                cConectorDiv = cConectorDivAux[0];
                            }

                            if (iDescripcionIndicador.classList.contains(iconPlus)) {
                                if (dDescripcionInd) {
                                    dDescripcionInd.style.display = 'none';
                                }

                                if (cConectorDiv) {
                                    cConectorDiv.style.display = 'none';
                                }
                            }
                            else if (iDescripcionIndicador.classList.contains(iconMinus)) {
                                if (dDescripcionInd) {
                                    dDescripcionInd.style.display = 'block';
                                }

                                if (cConectorDiv) {
                                    cConectorDiv.style.display = 'block';
                                }
                            }
                        }
                    }
                }
            }

            vm.fuenteConsultada = fuenteId;
            vm.politicaConsultada = politicaId;
            vm.productoConsultado = productoId;
            vm.localizacionConsultada = localizacionId;
        };

        /// Activar siguiente nivel detalle solicitud de recursos.
        vm.activarDescripcionAux = function (_this, nombreDivMuestra, conector, index, DetalleEtnicos, vigencia) {
            try {
                if (_this) {
                    var element = _this.currentTarget;
                    if (element) {
                        var iconPlus = 'iconPlusTercerNivel'; //'fa-plus-square';
                        var iconMinus = 'iconMinusTercerNivel'; //'fa-minus-square';
                        var eliminarColumna = false;
                        var iDescripcionIndicador = element;
                        if (iDescripcionIndicador) {

                            if (iDescripcionIndicador.classList.contains(iconMinus)) {
                                eliminarColumna = true;
                            }

                            iDescripcionIndicador.classList.toggle(iconPlus);
                            iDescripcionIndicador.classList.toggle(iconMinus);
                        }

                        var tblMain = iDescripcionIndicador.parentNode.parentNode.parentNode.parentNode;
                        if (tblMain && !eliminarColumna) {
                            /// Recalcular Indice.
                            var newIndex = 0;
                            if (vigencia) {
                                try {
                                    for (var ie = 0; ie < tblMain.rows.length; ie++) {
                                        if (tblMain.rows[ie].cells.length > 1) {
                                            var cell = tblMain.rows[ie].cells[1].getElementsByTagName("input");
                                            if (cell && cell.length > 0) {
                                                if (cell[0].value === vigencia.toString()) {
                                                    newIndex = ie;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } catch (e) {
                                    console.log(e);
                                }

                                if (index !== newIndex) {
                                    index = newIndex;
                                }
                            }

                            var rowActual = tblMain.rows[index];
                            var ind = index += 1;
                            var rowNew = tblMain.insertRow(ind);
                            var cell1 = rowNew.insertCell(0);
                            cell1.colSpan = '7';
                            cell1.style = 'border-left: 2px solid white; border - right: 2px solid white; ';

                            var nombBtnActEditSolRec = "btnActEditSolRec" + index;
                            var nombBtnActGuardarSolRec = "btnActGuardarSolRec" + index;
                            var css = `
                            <style>
                                .divNivel2${index} {
                                        border: 2px solid #3366cc;
                                        border-radius: 5px 0px;
                                        padding: 10px;
                                        margin: 1px 0 0 19.3%;
                                        border-bottom: none;
                                        border-right: none;
                                        width: 81.3%;
                                        height: 55px;
                                }
                                .${conector} {
                                     border-top: none;
                                    border-right: none;
                                    border-left: 2px solid #3366cc;
                                    border-bottom: 2px solid #3366cc;
                                    position: absolute;
                                    margin: 5px 0 0 31px;
                                    width: 15.7%;
                                    height: 1.5%;
                                }
                            </style>`;

                            var nDivSolRec = 'dCPP2' + index;
                            var html = '';
                            var n = vm.obtenerNumeroAleatorio();

                            var valorSolicitudRecCat = '';
                            if (rowActual) {
                                var r = rowActual.cells[rowActual.cells.length - 1];
                                if (r) {
                                    var t = r.getElementsByTagName('input');
                                    if (t && t.length > 0) {
                                        valorSolicitudRecCat = t[0].value;
                                    }
                                }
                            }

                            html = `${css}
                            <div class="${nDivSolRec}" style="text-align: justify; display:block">
                                <div class="divNivel2${index}">
                                <div class="titleIP espacioTituloPrimerNivel" style=" font-weight:bold;">Detalle asociación grupos étnicos</div>
                                    <div style="float: right; margin: -27px 0 0 0;">
                                        <button id="${nombBtnActEditSolRec}" type="button"
                                        class="btneditarDNP" uib-tooltip="Clic para editar la información" ng-click="vm.ActivarEditar($event)">
                                            EDITAR
                                        </button>
                                        <button id="${nombBtnActGuardarSolRec}" type="button" class="btnguardarDisabledDNP"
                                                uib-tooltip="Guardar modificación"
                                                ng-click="vm.ActivarEditar($event)" disabled>
                                            GUARDAR
                                        </button>
                                    </div>
                                </div>
                                <div style="width: 90%;margin: 20px 0 0 11px;">
                                    <table class="tablaSolRecCategoria" style="width: 109%;">
                                        <thead>
                                            <tr>
                                                <th class="cTexto borderTabla">&nbsp;</th>
                                                <th class="cTexto cAlignLeft titulosBold">Solicitud Recursos para categoría &nbsp; <i class="fa fa-long-arrow-right" aria-hidden="true"></i></th>
                                                <th class="cTexto cAlignRight"><input type="text" class="cInputTblSolRecCategoria" style="color: black;" value="${valorSolicitudRecCat}" disabled /></th>
                                                <th class="cTexto borderTabla">&nbsp;</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                                <div id="${n}" style="margin: 10px 0 0 0; padding: 10px 10px 30px 10px;">
                                    <table class="tablaDetalleSolRec">
                                        <thead>
                                            <tr>
                                                <td class="cTexto borderTabla">&nbsp;</td>
                                                <td class="cTexto cAlignLeft">Grupo étnico</td>
                                                <td class="cTexto cAlignRight">Población MGA</td>
                                                <td class="cTexto cAlignRight">Costos MGA $</td>
                                                <td class="cTexto cAlignRight">Solicitud recursos por grupo étnico $</td>
                                            </tr>
                                         </thead><tbody>`;

                            if (DetalleEtnicos && DetalleEtnicos.length > 0) {
                                for (var i = 0; i < DetalleEtnicos.length; i++) {
                                    var g = DetalleEtnicos[i];
                                    html += `
                                    <tr>
                                        <td class="cTexto">&nbsp;</td>
                                        <td class="cAlignLeft cLetraNegra">${g.NombreGrupo}</td>
                                        <td><input type="text" class="cInputTblDetalleCatBloq2" value="${g.PoblacionMGA}" disabled /></td>
                                        <td><input type="text" class="cInputTblDetalleCatBloq2" value="${vm.formatValores(g.CostosMGA)}" disabled /></td>
                                        <td><input type="text" class="cInputTblDetalleCatBloq2 numeric"
                                        value="${vm.formatValores(g.SolicitudRecurso)}" ng-blur="vm.actualizaFilaN2($event)" allow-numbers-only disabled /></td>
                                    </tr>`;
                                }
                            }

                            html += `</tbody>
                                     <tfoot>
                                        <tr>
                                            <td class="borderTabla cSinBackground columAcc icontotaldnp"><span>=</span></td>
                                            <td class="cTextoBold textRightDNP"><span>Total $</span></td>
                                            <td class="cTextoBold"></td>
                                            <td class="cTextoBold"></td>
                                            <td class="cTextoBold"></td>
                                        </tr>
                                    </tfoot>
                                    </table>
                                </div>
                            </div>`;


                            cell1.innerHTML = html;

                            /// Actualzar angular.
                            try {
                                var el = angular.element(document.getElementById(n));
                                if (el) {
                                    $compile(el.contents())($scope);
                                }
                            } catch (e) { console.log(e); }


                            /// Mascara numero.
                            //if (typeof $('.numeric').mask !== 'undefined') {
                            //    $('.numeric').mask("#,##0.00", { reverse: true });
                            //}

                            /// Calcular valores totales tabla solicitud de recursos.
                            var q = cell1.getElementsByClassName(nDivSolRec);
                            if (q !== null && q.length > 0) {
                                vm.CalcularTotalesSolRecursos(q, 'tablaDetalleSolRec');
                            }

                            var btnActEdit = document.getElementById(nombBtnActEditSolRec);
                            if (btnActEdit) {
                                btnActEdit.onclick = (event) => {
                                    vm.ActivarEditarSolicitudRec(event, DetalleEtnicos, n);
                                };
                            }

                            var btnActGuardar = document.getElementById(nombBtnActGuardarSolRec);
                            if (btnActGuardar) {
                                btnActGuardar.onclick = (event) => {
                                    vm.ActivarEditarSolicitudRec(event, DetalleEtnicos, n);
                                };
                            }
                        }

                        if (tblMain && eliminarColumna) {
                            /// Recalcular Indice.
                            var newIndex2 = 0;
                            if (vigencia) {
                                for (var ie = 0; ie < tblMain.rows.length; ie++) {
                                    if (tblMain.rows[ie].cells.length > 1) {
                                        var cell = tblMain.rows[ie].cells[1].getElementsByTagName("input");
                                        if (cell && cell.length > 0) {
                                            if (cell[0].value === vigencia.toString()) {
                                                newIndex2 = ie;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (index !== newIndex2) {
                                    index = newIndex2;
                                }
                            }

                            var ind2 = index += 1;
                            tblMain.deleteRow(ind2);
                        }

                        var dDescripcionInd;
                        var dRepeat = iDescripcionIndicador.parentNode;
                        if (dRepeat) {
                            var d = dRepeat.getElementsByClassName(nombreDivMuestra);
                            if (d && d.length > 0) {
                                dDescripcionInd = d[0];
                            }
                        }

                        var cConectorDiv;
                        var cConectorDivAux = dRepeat.getElementsByClassName(conector);
                        if (cConectorDivAux && cConectorDivAux.length > 0) {
                            cConectorDiv = cConectorDivAux[0];
                        }

                        if (iDescripcionIndicador.classList.contains(iconPlus)) {
                            if (dDescripcionInd) {
                                dDescripcionInd.style.display = 'none';
                            }

                            if (cConectorDiv) {
                                cConectorDiv.style.display = 'none';
                            }
                        }
                        else if (iDescripcionIndicador.classList.contains(iconMinus)) {
                            if (dDescripcionInd) {
                                dDescripcionInd.style.display = 'block';
                            }

                            if (cConectorDiv) {
                                cConectorDiv.style.display = 'block';
                            }
                        }
                    }
                }
            } catch (e) {
                console.log(e);
            }
        };

        /// Activar boton de editar - Detalle Vigencia.
        vm.ActivarEditar = function (_this) {
            var v = _this.currentTarget ? _this.currentTarget.innerText : '';
            if (v) {
                if (v === 'EDITAR') {
                    _this.currentTarget.innerText = _this.currentTarget.innerHTML = 'CANCELAR';

                    var parent = _this.currentTarget.parentNode.parentNode.parentNode;
                    var tablaDetalleVigencia = parent.getElementsByClassName('tablaDetalleVigencia');
                    if (tablaDetalleVigencia && tablaDetalleVigencia.length > 0) {
                        var tDetVigencia = tablaDetalleVigencia[0];
                        for (var i = 1; i < tDetVigencia.rows.length - 1; i++) {
                            var input = tDetVigencia.rows[i].cells[tDetVigencia.rows[i].cells.length - 1].getElementsByTagName('input');
                            var divinfo = tDetVigencia.rows[i].cells[tDetVigencia.rows[i].cells.length - 1].getElementsByTagName('div');
                            if (input && input.length > 0) {
                                if (!input[0].classList.contains("cInputTblSolRecCategoria")) {
                                    input[0].disabled = false;
                                    divinfo[0].hidden = true;
                                    input[0].style.display = 'block';
                                    input[0].classList.toggle("cInputTblDetalleCat");
                                    input[0].classList.toggle("inputCPP2");
                                    var td = input[0].parentNode;
                                    if (td) {
                                        td.classList.toggle("cInputTblDetalleCat");
                                    }
                                }
                            }
                        }
                    }

                    var parentDiv = _this.currentTarget.parentNode;
                    if (parentDiv) {
                        var btnguardarDisabledDNP = parentDiv.getElementsByClassName('btnguardarDisabledDNP');
                        if (btnguardarDisabledDNP && btnguardarDisabledDNP.length > 0) {
                            btnguardarDisabledDNP[0].disabled = false;
                            btnguardarDisabledDNP[0].classList.add("btnguardarDNP");
                            btnguardarDisabledDNP[0].classList.remove("btnguardarDisabledDNP");
                        }
                    }
                }

                if (v === 'CANCELAR') {
                    _this.currentTarget.innerText = _this.currentTarget.innerHTML = 'EDITAR';
                    var parent2 = _this.currentTarget.parentNode.parentNode.parentNode;
                    if (parent2) {
                        vm.editarNoEditarDetalleVigencia(parent2,
                            'tablaDetalleVigencia', 'cInputTblDetalleCatBloq');
                    }

                    var parentDiv = _this.currentTarget.parentNode;
                    if (parentDiv) {
                        var btnguardarDNP = parentDiv.getElementsByClassName('btnguardarDNP');
                        if (btnguardarDNP && btnguardarDNP.length > 0) {
                            btnguardarDNP[0].disabled = true;
                            btnguardarDNP[0].classList.add("btnguardarDisabledDNP");
                            btnguardarDNP[0].classList.remove("btnguardarDNP");
                        }
                    }
                }

                /// Se guarda la información de solicitud de recursos.
                if (v === 'GUARDAR') {
                    var cat = vm.catProdPolitica;
                    for (var i = 0; i < cat.Politicas.length; i++) {
                        for (var ij = 0; ij < cat.Politicas[i].Productos.length; ij++) {
                            var localizaciones = cat.Politicas[i].Productos[ij].Localizaciones;
                            if (localizaciones && localizaciones.length > 0) {
                                for (var ix = 0; ix < localizaciones.length; ix++) {
                                    /// Formato datos.
                                    var categorias = localizaciones[ix].Categorias;
                                    if (categorias) {
                                        for (var j11 = 0; j11 < categorias.length; j11++) {
                                            var categoria = categorias[j11];
                                            if (categoria && categoria.Vigencias) {
                                                for (var j12 = 0; j12 < categoria.Vigencias.length; j12++) {
                                                    var vigencia = categoria.Vigencias[j12];
                                                    if (vigencia && vigencia.SolicitudRecursosCategoria) {
                                                        var v1 = parseFloat(vigencia.SolicitudRecursosCategoria.toString().replace(",", "."));
                                                        vigencia.SolicitudRecursosCategoria = v1;
                                                    }


                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var parent1 = _this.currentTarget.parentNode.parentNode.parentNode;
                    vm.guardarSolicitudRecurso(vm.catProdPolitica, _this.currentTarget, '1', parent1);
                }
            }
        };

        /// Calculo de totales tabla detalle Vigencia.
        vm.CalcularTotales = function (_this) {
            if (_this && _this.currentTarget) {
                var ct = _this.currentTarget;
                var colapsado = ct && ct.classList.contains('iconMinusTercerNivel') ? true : false;
                var parent = _this.currentTarget.parentNode.parentNode;
                var dCPP2 = parent.getElementsByClassName('dCPP2');
                if (colapsado && dCPP2 && dCPP2.length > 0) {
                    var tablaDetalleVigencia = dCPP2[0].getElementsByClassName('tablaDetalleVigencia');
                    if (tablaDetalleVigencia && tablaDetalleVigencia.length > 0) {
                        vm.CalcularTotalesAux(tablaDetalleVigencia[0]);
                    }
                }
            }
        };

        /// Complemento - Calculo de totales tabla detalle Vigencia.
        vm.CalcularTotalesAux = function (tablaDetalleVigencia) {
            if (tablaDetalleVigencia) {
                var tDetVigencia = tablaDetalleVigencia;
                var sumaDetVigencia = [];
                if (tDetVigencia) {
                    for (var i = 1; i < tDetVigencia.rows.length - 1; i++) {
                        if (tDetVigencia.rows[i].cells && tDetVigencia.rows[i].cells.length > 4) {
                            var inicioCellDatos = 2;
                            var costosVigMGA = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 1,
                                valor: costosVigMGA ? costosVigMGA.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });

                            inicioCellDatos += 1;
                            var costosCatMGA = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 2,
                                valor: costosCatMGA ? costosCatMGA.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });

                            inicioCellDatos += 1;
                            var metaCatMGA = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 3,
                                valor: metaCatMGA ? metaCatMGA.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });

                            inicioCellDatos += 1;
                            var costosFuentReg = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 4,
                                valor: costosFuentReg ? costosFuentReg.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });

                            inicioCellDatos += 1;
                            var solRecCat = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 5,
                                valor: solRecCat ? solRecCat.replace(",", ".") : ''
                            });

                        } else {
                            /// Si está expandida opcion de grupos etnicos, actualiza el valor de solicitud
                            var rowAnterior = i - 1;
                            if (rowAnterior > 0 && tDetVigencia.rows[rowAnterior].cells) {
                                var row = tDetVigencia.rows[rowAnterior];
                                var v1 = '';
                                if (row) {
                                    var cell1 = row.cells[row.cells.length - 1];
                                    if (cell1) {
                                        var input1 = cell1.getElementsByTagName("input");
                                        if (input1 && input1.length > 0) {
                                            v1 = input1[0].value;
                                        }
                                    }
                                }

                                var rowAct = tDetVigencia.rows[i];
                                if (rowAct && v1) {
                                    var c = rowAct.cells[rowAct.cells.length - 1];
                                    if (c) {
                                        var u = c.getElementsByTagName("input");
                                        if (u && u.length > 0) {
                                            u[0].value = v1;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (sumaDetVigencia.length > 0) {
                        var sCostosVigMGA = sumaDetVigencia.filter(a => a.tipo === 1);
                        var sCostosCatMGA = sumaDetVigencia.filter(a => a.tipo === 2);
                        var sMetaCatMGA = sumaDetVigencia.filter(a => a.tipo === 3);
                        var sCostosFuentReg = sumaDetVigencia.filter(a => a.tipo === 4);
                        var sSolRecCat = sumaDetVigencia.filter(a => a.tipo === 5);

                        if (sCostosVigMGA) {
                            var sumarTotalCVMGA = 0;
                            var inicioCellDatos2 = 2;

                            for (var v1 = 0; v1 < sCostosVigMGA.length; v1++) {
                                sumarTotalCVMGA += sCostosVigMGA[v1] && sCostosVigMGA[v1].valor ? parseFloat(sCostosVigMGA[v1].valor) : 0;
                            }
                            var cTotalCostosVigMGA = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostosVigMGA) {
                                cTotalCostosVigMGA.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq3" value="${vm.formatValores(sumarTotalCVMGA.toString())}" disabled />`;
                            }
                        }

                        if (sCostosCatMGA) {
                            var sumarTotalCCMGA = 0;
                            for (var v2 = 0; v2 < sCostosCatMGA.length; v2++) {
                                sumarTotalCCMGA += sCostosCatMGA[v2] && sCostosCatMGA[v2].valor ? parseFloat(sCostosCatMGA[v2].valor) : 0;
                            }
                            inicioCellDatos2 = inicioCellDatos2 + 1;
                            var cTotalCostosCMGA = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostosCMGA) {
                                cTotalCostosCMGA.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq3" value="${vm.formatValores(sumarTotalCCMGA.toString())}" disabled />`;
                            }
                        }

                        if (sMetaCatMGA) {
                            var sumarTotal3 = 0;
                            for (var v3 = 0; v3 < sMetaCatMGA.length; v3++) {
                                sumarTotal3 += sMetaCatMGA[v3] && sMetaCatMGA[v3].valor ? parseFloat(sMetaCatMGA[v3].valor) : 0;
                            }
                            inicioCellDatos2 = inicioCellDatos2 + 1;
                            var cTotalCostos3 = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostos3) {
                                //cTotalCostos3.innerHTML = '<span class="cSpTotalTbl">' + vm.formatValores(sumarTotal3.toString()) + '</span>';
                                cTotalCostos3.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq3" value="${vm.formatValores(sumarTotal3.toString())}" disabled />`;
                            }
                        }

                        if (sCostosFuentReg) {
                            var sumarTotal4 = 0;
                            for (var v4 = 0; v4 < sCostosFuentReg.length; v4++) {
                                sumarTotal4 += sCostosFuentReg[v4] && sCostosFuentReg[v4].valor ? parseFloat(sCostosFuentReg[v4].valor) : 0;
                            }
                            inicioCellDatos2 = inicioCellDatos2 + 1;
                            var cTotalCostos4 = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostos4) {
                                cTotalCostos4.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq3" value="${vm.formatValores(sumarTotal4.toString())}" disabled />`;
                            }
                        }

                        if (sSolRecCat) {
                            var sumarTotal5 = 0;
                            for (var v5 = 0; v5 < sSolRecCat.length; v5++) {
                                sumarTotal5 += sSolRecCat[v5] && sSolRecCat[v5].valor ? parseFloat(sSolRecCat[v5].valor) : 0;
                            }
                            inicioCellDatos2 = inicioCellDatos2 + 1;
                            var cTotalCostos5 = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostos5) {
                                cTotalCostos5.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq3" value="${vm.formatValores(sumarTotal5.toString())}" disabled />`;
                            }
                        }
                    }
                }
            }
        };

        /// Calculo de totales tabla detalle recuros y grupos etnicos.
        vm.CalcularTotales2 = function (_this) {
            if (_this) {
                var tablaDetalleSolRec = document.getElementsByClassName(_this);
                if (tablaDetalleSolRec && tablaDetalleSolRec.length > 0) {
                    var tablaDetalleSolRec1 = tablaDetalleSolRec[0].getElementsByClassName('tablaDetalleSolRec');
                    if (tablaDetalleSolRec1 && tablaDetalleSolRec1.length > 0) {
                        vm.CalcularTotalesSolRecursosAux(tablaDetalleSolRec1[0]);
                    }
                }
            }
        };

        /// Activar boton de editar.
        vm.ActivarEditarSolicitudRec = function (_this, DetalleEtnicos, nControl) {
            var v = _this.currentTarget ? _this.currentTarget.innerText : '';
            if (v) {
                if (v === 'EDITAR') {
                    _this.currentTarget.innerText = _this.currentTarget.innerHTML = 'CANCELAR';

                    var parent = _this.currentTarget.parentNode.parentNode.parentNode;
                    var tablaDetalleVigencia = parent.getElementsByClassName('tablaDetalleSolRec');
                    if (tablaDetalleVigencia && tablaDetalleVigencia.length > 0) {
                        var tDetVigencia = tablaDetalleVigencia[0];
                        for (var i = 1; i < tDetVigencia.rows.length - 1; i++) {
                            var input = tDetVigencia.rows[i].cells[tDetVigencia.rows[i].cells.length - 1].getElementsByTagName('input');
                            if (input && input.length > 0) {
                                input[0].disabled = false;
                                input[0].classList.toggle("cInputTblDetalleCat3");
                                input[0].classList.toggle("inputCPP3");
                            }
                        }
                    }

                    var parentDiv = _this.currentTarget.parentNode;
                    if (parentDiv) {
                        var btnguardarDisabledDNP = parentDiv.getElementsByClassName('btnguardarDisabledDNP');
                        if (btnguardarDisabledDNP && btnguardarDisabledDNP.length > 0) {
                            btnguardarDisabledDNP[0].disabled = false;
                            btnguardarDisabledDNP[0].classList.add("btnguardarDNP");
                            btnguardarDisabledDNP[0].classList.remove("btnguardarDisabledDNP");
                        }
                    }
                }
                //if (v === 'EDITAR') {
                //    _this.currentTarget.innerText = _this.currentTarget.innerHTML = 'GUARDAR';

                //    _this.currentTarget.classList.remove("btnguardarDNP");
                //    _this.currentTarget.classList.remove("btneditarDNP");
                //    _this.currentTarget.classList.add("btnguardarDNP");

                //    var parent = _this.currentTarget.parentNode.parentNode.parentNode;
                //    var tablaDetalleVigencia = parent.getElementsByClassName('tablaDetalleSolRec');
                //    if (tablaDetalleVigencia && tablaDetalleVigencia.length > 0) {
                //        var tDetVigencia = tablaDetalleVigencia[0];
                //        for (var i = 1; i < tDetVigencia.rows.length - 1; i++) {
                //            var input = tDetVigencia.rows[i].cells[tDetVigencia.rows[i].cells.length - 1].getElementsByTagName('input');
                //            if (input && input.length > 0) {
                //                input[0].disabled = false;
                //                input[0].classList.toggle("cInputTblDetalleCat3");
                //                input[0].classList.toggle("inputCPP3");
                //            }
                //        }
                //    }
                //} else {
                //    _this.currentTarget.classList.remove("btnguardarDNP");
                //    _this.currentTarget.classList.remove("btneditarDNP");
                //    _this.currentTarget.classList.add("btneditarDNP");
                //}

                if (v === 'CANCELAR') {
                    _this.currentTarget.innerText = _this.currentTarget.innerHTML = 'EDITAR';
                    var parent2 = _this.currentTarget.parentNode.parentNode.parentNode;
                    if (parent2) {
                        vm.editarNoEditarDetalleVigencia(parent2,
                            'tablaDetalleSolRec', 'cInputTblDetalleCatBloq2');
                    }

                    var parentDiv = _this.currentTarget.parentNode;
                    if (parentDiv) {
                        var btnguardarDNP = parentDiv.getElementsByClassName('btnguardarDNP');
                        if (btnguardarDNP && btnguardarDNP.length > 0) {
                            btnguardarDNP[0].disabled = true;
                            btnguardarDNP[0].classList.add("btnguardarDisabledDNP");
                            btnguardarDNP[0].classList.remove("btnguardarDNP");
                        }
                    }
                }

                /// Se guarda la información de solicitud de recursos.
                if (v === 'GUARDAR') {
                    //_this.currentTarget.classList.remove("btnguardarDNP");
                    //_this.currentTarget.classList.remove("btneditarDNP");
                    //_this.currentTarget.classList.add("btnguardarDNP");

                    //utilidades.mensajeError("Debe ingresar un Bpin...", false);
                    if (nControl && DetalleEtnicos) {
                        var t = document.getElementById(nControl).getElementsByTagName("table");
                        if (t && t.length > 0) {
                            var t1 = t[0];
                            if (t1) {
                                for (var i = 1; i < t1.rows.length; i++) {
                                    var r = t1.rows[i].cells[t1.rows[i].cells.length - 1];
                                    var inputs = r.getElementsByTagName("input");
                                    if (inputs && inputs.length > 0) {
                                        var valor = inputs[0].value;
                                        var y = i - 1;
                                        if (DetalleEtnicos.length > y) {
                                            var v1 = parseInt(valor.replace(/\./g, "")); //.replace(/\,/g, "."));
                                            DetalleEtnicos[y].SolicitudRecurso = v1;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var parent1 = _this.currentTarget.parentNode.parentNode.parentNode;
                    vm.guardarSolicitudRecurso(vm.catProdPolitica, _this.currentTarget, '2', parent1);
                }
            }
        };

        /// Calculo de totales tabla Solicitud de recursos.
        vm.CalcularTotalesSolRecursos = function (c1, t1) {
            try {
                if (c1) {
                    var dCPP2 = c1; // document.getElementsByClassName(c1);
                    if (dCPP2 && dCPP2.length > 0) {
                        var tablaDetalleSolRec = dCPP2[0].getElementsByClassName(t1);
                        if (tablaDetalleSolRec && tablaDetalleSolRec.length > 0) {
                            var tDetVigencia = tablaDetalleSolRec[0];
                            vm.CalcularTotalesSolRecursosAux(tDetVigencia);
                        }
                    }
                }
            } catch (e) {
                console.log(e);
            }
        };

        /// Calculo de totales tabla Solicitud de recursos - Auxiliar
        vm.CalcularTotalesSolRecursosAux = function (tDetVigencia) {
            try {
                var sumaDetVigencia = [];
                if (tDetVigencia && tDetVigencia.rows.length > 2) {
                    for (var i = 1; i < tDetVigencia.rows.length - 1; i++) {
                        if (tDetVigencia.rows[i].cells && tDetVigencia.rows[i].cells.length > 3) {
                            var inicioCellDatos = 2;
                            var poblacionMGA = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 1,
                                valor: poblacionMGA ? poblacionMGA.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });

                            inicioCellDatos += 1;
                            var costosMGA = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 2,
                                valor: costosMGA ? costosMGA.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });

                            inicioCellDatos += 1;
                            var solRecMGA = tDetVigencia.rows[i].cells[inicioCellDatos].getElementsByTagName('input')[0].value;
                            sumaDetVigencia.push({
                                tipo: 3,
                                valor: solRecMGA ? solRecMGA.replace(/\./g, '').replace(/\,/g, '.') : ''
                            });
                        }
                    }

                    if (sumaDetVigencia.length > 0) {
                        var sPoblacionMGA = sumaDetVigencia.filter(a => a.tipo === 1);
                        var sCostosMGA = sumaDetVigencia.filter(a => a.tipo === 2);
                        var sSolrec = sumaDetVigencia.filter(a => a.tipo === 3);

                        if (sPoblacionMGA) {
                            var sumarTotalPob = 0;
                            var inicioCellDatos2 = 2;

                            for (var v1 = 0; v1 < sPoblacionMGA.length; v1++) {
                                sumarTotalPob += sPoblacionMGA[v1].valor ? parseFloat(sPoblacionMGA[v1].valor) : 0;
                            }
                            var cTotalCostosVigMGA = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostosVigMGA && !isNaN(sumarTotalPob)) {
                                cTotalCostosVigMGA.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq4" value="${vm.formatValores(sumarTotalPob.toString())}" disabled />`;
                            }
                        }

                        if (sCostosMGA) {
                            var sumarTotalCMGA = 0;
                            for (var v2 = 0; v2 < sCostosMGA.length; v2++) {
                                sumarTotalCMGA += sCostosMGA[v2].valor ? parseFloat(sCostosMGA[v2].valor) : 0;
                            }
                            inicioCellDatos2 = inicioCellDatos2 + 1;
                            var cTotalCostosMGA = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostosMGA && !isNaN(sumarTotalCMGA)) {
                                cTotalCostosMGA.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq4" value="${vm.formatValores(sumarTotalCMGA.toString())}" disabled />`;
                            }
                        }

                        if (sSolrec) {
                            var sumarTotal3 = 0;
                            for (var v3 = 0; v3 < sSolrec.length; v3++) {
                                sumarTotal3 += sSolrec[v3].valor ? parseFloat(sSolrec[v3].valor) : 0;
                            }
                            inicioCellDatos2 = inicioCellDatos2 + 1;
                            var cTotalCostos3 = tDetVigencia.rows[tDetVigencia.rows.length - 1].cells[inicioCellDatos2];
                            if (cTotalCostos3) {
                                cTotalCostos3.innerHTML = `<input type="text" class="cInputTblDetalleCatBloq4" value="${vm.formatValores(sumarTotal3.toString())}" disabled />`;
                            }
                        }
                    }
                }
            } catch (e) {
                console.log(e);
            }
        };

        /// Formatear titulo localizacion.
        vm.tituloLocalizacion = function (obj) {
            var titulo = '';
            if (obj) {

                if (obj.Departamento) {
                    titulo = obj.Departamento;
                }

                if (obj.Municipio) {
                    titulo += ' - ' + obj.Municipio;
                }

                if (obj.TipoAgrupacion) {
                    titulo += ' - ' + obj.TipoAgrupacion;
                }

                if (obj.Agrupacion) {
                    titulo += ' - ' + obj.Agrupacion;
                }

                if (titulo && titulo.indexOf('-') > 0 && titulo.indexOf('-') <= 1) {
                    titulo = titulo.substring(2);
                }
            }

            return titulo;
        };

        /// Guardar datos de solicitud de recursos.
        vm.guardarSolicitudRecurso = function (dat, currentTarget, tipo, currentTable) {
            gestionRecursosSGPServicio
                .guardarDatosSolicitudRecursosSgp(dat)
                .then(
                    (respuesta) => {
                        if (respuesta.data != null && respuesta.data != "") {
                            var mensajeGuardado = '';

                            if (tipo === '1') {
                                mensajeGuardado = 'Información guardada satisfactoriamente';
                            }

                            if (tipo === '2') {
                                mensajeGuardado = 'Los datos fueron guardados con éxito';
                            }

                            utilidades.mensajeSuccess(mensajeGuardado, false, false, false, "Los datos fueron guardados con éxito.");

                            if (currentTarget && currentTarget.classList) {
                                var nombreTabla = '';
                                if (tipo === '1') {
                                    vm.editarNoEditarDetalleVigencia(currentTable,
                                        'tablaDetalleVigencia', 'cInputTblDetalleCatBloq');
                                    nombreTabla = 'tablaDetalleVigencia';

                                    /// Reajustar formato muestra.
                                    var cat = dat;
                                    for (var i = 0; i < cat.Politicas.length; i++) {
                                        for (var ij = 0; ij < cat.Politicas[i].Productos.length; ij++) {
                                            var localizaciones = cat.Politicas[i].Productos[ij].Localizaciones;
                                            if (localizaciones && localizaciones.length > 0) {
                                                for (var ix = 0; ix < localizaciones.length; ix++) {
                                                    /// Formato datos.
                                                    var categorias = localizaciones[ix].Categorias;
                                                    if (categorias) {
                                                        for (var j11 = 0; j11 < categorias.length; j11++) {
                                                            var categoria = categorias[j11];
                                                            if (categoria && categoria.Vigencias) {
                                                                for (var j12 = 0; j12 < categoria.Vigencias.length; j12++) {
                                                                    var vigencia = categoria.Vigencias[j12];
                                                                    if (vigencia) {
                                                                        vigencia.SolicitudRecursosCategoria = vigencia.SolicitudRecursosCategoria;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (tipo === '2') {
                                    vm.editarNoEditarDetalleVigencia(currentTable,
                                        'tablaDetalleSolRec', 'cInputTblDetalleCatBloq2');
                                    nombreTabla = 'tablaDetalleSolRec';
                                }

                                /// Botones editar / Guardar.
                                var parentDiv = currentTarget.parentNode;
                                if (parentDiv) {
                                    var btnguardarDNP = parentDiv.getElementsByClassName('btnguardarDNP');
                                    if (btnguardarDNP && btnguardarDNP.length > 0) {
                                        btnguardarDNP[0].disabled = true;
                                        btnguardarDNP[0].classList.add("btnguardarDisabledDNP");
                                        btnguardarDNP[0].classList.remove("btnguardarDNP");
                                    }

                                    var btneditarDNP = parentDiv.getElementsByClassName('btneditarDNP');
                                    if (btneditarDNP && btneditarDNP.length > 0) {
                                        btneditarDNP[0].innerHTML = 'EDITAR';
                                    }
                                }

                                if (nombreTabla) {
                                    var tablaDetalleVigencia = currentTable.getElementsByClassName(nombreTabla);
                                    if (tablaDetalleVigencia && tablaDetalleVigencia.length > 0) {
                                        if (tipo === '1') {
                                            vm.CalcularTotalesAux(tablaDetalleVigencia[0]);

                                            /// Actualizar valor Categorias - Costo total categoria.
                                            var e1 = currentTable.closest('.dMainCategoria');
                                            if (e1) {
                                                try {
                                                    var y1 = e1.getElementsByClassName('inpCostoTotalEnRec');
                                                    if (y1 && y1.length > 0) {
                                                        var inpCostoTotalRec = y1[0];
                                                        if (inpCostoTotalRec) {
                                                            var tbl1 = tablaDetalleVigencia[0];
                                                            if (tbl1 && tbl1.rows.length > 0) {
                                                                var cell1 = tbl1.rows[tbl1.rows.length - 1].cells[tbl1.rows[tbl1.rows.length - 1].cells.length - 1].getElementsByTagName("input");
                                                                if (cell1) {
                                                                    var valorTotal = cell1[0].value;
                                                                    if (valorTotal) {
                                                                        inpCostoTotalRec.value = valorTotal;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                } catch (e) { }
                                            }
                                        }
                                        if (tipo === '2') {
                                            vm.CalcularTotalesSolRecursosAux(tablaDetalleVigencia[0]);
                                        }
                                    }
                                }
                            }

                            guardarCapituloModificado();
                        }
                    }).catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación.");
                            return;
                        }
                        utilidades.mensajeError("Error al realizar la operación.");
                    });
        };

        vm.editarNoEditarDetalleVigencia = function (currentTarget, tablaNombre, claseNombre) {
            var parent = currentTarget;
            var tablaDetalleVigencia = parent.getElementsByClassName(tablaNombre);
            if (tablaDetalleVigencia && tablaDetalleVigencia.length > 0) {
                var tDetVigencia = tablaDetalleVigencia[0];
                for (var i = 1; i < tDetVigencia.rows.length - 1; i++) {
                    var input = tDetVigencia.rows[i].cells[tDetVigencia.rows[i].cells.length - 1].getElementsByTagName('input');
                    if (input && input.length > 0) {
                        input[0].disabled = true;
                        input[0].classList.remove('cInputTblDetalleCat');
                        input[0].classList.remove('inputCPP2');

                        input[0].classList.remove('cInputTblDetalleCat3');
                        input[0].classList.remove('inputCPP3');

                        input[0].classList.remove(claseNombre);
                        input[0].classList.add(claseNombre);
                        if (tablaNombre == "tablaDetalleVigencia") {
                            var divinfo = tDetVigencia.rows[i].cells[tDetVigencia.rows[i].cells.length - 1].getElementsByTagName('div');
                            divinfo[0].hidden = false;
                            input[0].style.display = 'none';
                        }
                    }
                }
            }
        };

        /// Guardar datos de solicitud de recursos.
        vm.imprimirMensaje = function (mensaje) {
            var cMensajeCatPolitica = document.getElementsByClassName('cMensajeCatPolitica');
            if (cMensajeCatPolitica && cMensajeCatPolitica.length > 0) {
                var cMensaje = cMensajeCatPolitica[0];
                var cMensajeCatPol = cMensaje.getElementsByClassName('cMensajeCatPol');
                if (cMensajeCatPol && cMensajeCatPol.length > 0) {
                    cMensaje.style.display = 'block';
                    cMensajeCatPol[0].innerHTML = mensaje;
                }
            }
        };

        vm.cerrarModal = function (id) {
            $('#' + id).modal('hide');
        };

        /// Numero aleatorio.
        vm.obtenerNumeroAleatorio = function () {
            return Math.floor(Math.random() * 10000);
        };

        /// Filtrar vista de datos.
        vm.filtrarDatosResult = function (politicaId) {
            var secCategoriaProductosPolitica = document.getElementById('secCategoriaProductosPolitica');
            if (secCategoriaProductosPolitica && politicaId) {
                var s = secCategoriaProductosPolitica.getElementsByClassName('cDivMainCatProdPol');
                if (s && s.length > 0) {
                    var encontro = false;
                    for (var i = 0; i < s.length; i++) {
                        var d = s[i];
                        var att = d.getAttribute("data-attribute");
                        if (att && att !== politicaId.toString()) {
                            d.style.display = 'none';
                        }
                        else {
                            d.style.display = 'block';
                            encontro = true;
                        }
                    }

                    var cMensajeCatProdPol = secCategoriaProductosPolitica.getElementsByClassName('cMensajeCatProdPol');
                    /// No tiene datos.
                    if (!encontro) {
                        if (cMensajeCatProdPol) {
                            cMensajeCatProdPol[0].innerHTML = '¡Sin datos de consulta!.';
                        }
                    } else {
                        if (cMensajeCatProdPol) {
                            cMensajeCatProdPol[0].innerHTML = '';
                        }
                    }
                }
            }
        };

        /// Poner el mayuscula la primera letra.
        vm.capitalizarPrimeraLetra = function (str) {
            str = str.toLowerCase();
            return str.charAt(0).toUpperCase() + str.slice(1);
        };

        //Object.defineProperty(String.prototype, 'capitalizarPrimeraLetra', {
        //    value: function () {
        //        return this.charAt(0).toUpperCase() + this.slice(1);
        //    },
        //    writable: true, // Asi, puede sobreescribirse más tarde
        //    configurable: true // Asi, puede borrarse más tarde
        //});

        $scope.$on("obtenerdatoscpp", function (event, data) {
            //$event.stopPropagation();
            event.preventDefault();
            vm.obtenerDatosCPP(vm.BPIN, data.fuenteId, data.politicaId);
        });

        /// Formatear nombre de la localizacion.
        vm.formatoNombreLocalizacion = function (nombre) {
            nombre = nombre.trim();
            if (nombre && nombre.endsWith('-')) {
                nombre = nombre.substring(0, nombre.length - 1);
                vm.formatoNombreLocalizacion(nombre);
            } else {
                return nombre;
            }

            return nombre;
        };

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 1 && n[1] > 999) || (n[1].length > 1 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 16) {
                    event.preventDefault();
                }
            }
        };

        vm.validarTamanio = function (event) {
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
            var tamanioPermitido = 24;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 24;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
                        event.preventDefault();
                    }
                }
            }
        }

        vm.actualizaFila = function (event) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            var _this = event;
            if (_this && _this.currentTarget) {
                var ct = _this.currentTarget;
                var tablaDetalleVigencia = ct.closest(".tablaDetalleVigencia");
                if (tablaDetalleVigencia) {
                    if (tablaDetalleVigencia) {
                        vm.CalcularTotalesAux(tablaDetalleVigencia);
                    }
                }
            }

            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        ///
        vm.actualizaFilaN2 = function (event) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            var _this = event;
            if (_this && _this.currentTarget) {
                var ct = _this.currentTarget;
                var tablaDetalleVigencia = ct.closest(".tablaDetalleSolRec");
                if (tablaDetalleVigencia) {
                    if (tablaDetalleVigencia) {
                        vm.CalcularTotalesSolRecursosAux(tablaDetalleVigencia);
                    }
                }
            }
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        $scope.$watch('vm.cargado', function () {
            if (vm.valido === "true") {
                setTimeout(() => vm.notificacionValidacionPadre(JSON.parse(vm.listaerrores)), 2500);
                //vm.notificacionValidacionPadre(JSON.parse(vm.listaerrores));
            }
        });

        /* ------------------------ Validaciones ---------------------------------*/



        vm.notificacionValidacionPadre = function (errores) {

            vm.limpiarErrores(errores);
            if (errores != undefined) {

                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl !== undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }

                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

                //FUENTE_001
            }
        }



        vm.validarCAT_FUENTE_001 = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-focalizaciongrcapitulo1-" + errores);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }

            var idSpanAlertComponent = document.getElementById("alert-focalizaciongr");
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }
        }

        vm.validarCAT_POLITICA_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            var campoObligatorioJustificacion = document.getElementById("idCategoria-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('divInconsistencia');
            }

        }


        vm.validarCAT_PRODUCTO_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-PRODUCTOCA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarCAT_LOCALIZACION_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-LOCALIZACIONCA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarCAT_DIMENSION_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-DIMENSIONCA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarCAT_VIGENCIA_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-VIGENCIACA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            var campoObligatorioJustificacion = document.getElementById("idinput-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('divInconsistencia');
            }
        }

        vm.validarCAT_GENERAL_001 = function (errores) {
            const arreglo = errores.split('-');
            let fuente = arreglo[0];
            let politica = arreglo[1];
            let mensaje = arreglo[2];
            var temp = "";
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + '-' + fuente + '-' + politica + '-' + "pregunta-error1");
            if (campoObligatorioJustificacion != undefined) {
                temp = temp + campoObligatorioJustificacion.innerHTML;
                campoObligatorioJustificacion.innerHTML = temp + "<span class='d-inline-block ico-advertencia'></span><span>" + mensaje + "</span><br/>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.validarCAT_GENERAL_002 = function (errores) {

            const arreglo = errores.split('-');
            let fuente = arreglo[0];
            let politica = arreglo[1];
            let mensaje = arreglo[2];
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + '-' + fuente + '-' + politica + '-' + "pregunta-error2");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + mensaje + "</span>";

                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.validarERR_DETALLE_POLITICA_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            var campoObligatorioJustificacion = document.getElementById("idCruce-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('divInconsistencia');
            }
        }


        vm.limpiarErrores = function () {

            var idSpanAlertComponent = document.getElementById("alert-focalizaciongrcapitulo1-" + vm.fuenteId);
            if (idSpanAlertComponent != undefined) {

                idSpanAlertComponent.classList.remove("ico-advertencia");

            }
            var idSpanAlertComponent = document.getElementById("alert-focalizaciongr");
            if (idSpanAlertComponent != undefined) {

                idSpanAlertComponent.classList.remove("ico-advertencia");

            }


            if ($scope.datos !== undefined)
                $scope.datos.forEach(pp => {

                    pp.Productos.forEach(p => {

                        var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + '-' + vm.fuenteId + '-' + pp.PoliticaId + '-' + "pregunta-error1");
                        if (campoObligatorioProyectos != undefined) {
                            campoObligatorioProyectos.innerHTML = "";
                            campoObligatorioProyectos.classList.add('hidden');
                        }

                        var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + '-' + vm.fuenteId + '-' + pp.PoliticaId + '-' + "pregunta-error2");
                        if (campoObligatorioProyectos != undefined) {
                            campoObligatorioProyectos.innerHTML = "";
                            campoObligatorioProyectos.classList.add('hidden');
                        }

                        var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICA-" + vm.fuenteId + '-' + pp.PoliticaId);
                        if (campoObligatorioJustificacion != undefined) {
                            campoObligatorioJustificacion.innerHTML = "";
                            campoObligatorioJustificacion.classList.add('hidden');
                        }
                        var campoObligatorioJustificacion = document.getElementById("idCategoria-" + vm.fuenteId + '-' + pp.PoliticaId);
                        if (campoObligatorioJustificacion != undefined) {
                            campoObligatorioJustificacion.classList.remove('divInconsistencia');
                        }
                        var campoObligatorioJustificacion = document.getElementById("idCruce-" + vm.fuenteId + '-' + pp.PoliticaId);
                        if (campoObligatorioJustificacion != undefined) {
                            campoObligatorioJustificacion.classList.remove('divInconsistencia');
                        }




                        //focalizaciongrcapitulo1-PRODUCTOCA-{{vm.fuenteId}}-{{dat.PoliticaId}}-{{prod.ProductoId}
                        var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-PRODUCTOCA-" + vm.fuenteId + "-" + pp.PoliticaId + "-" + p.ProductoId);
                        if (campoObligatorioProyectos != undefined) {
                            campoObligatorioProyectos.innerHTML = "";
                            campoObligatorioProyectos.classList.add('hidden');
                        }
                        p.Localizaciones.forEach(l => {
                            //focalizaciongrcapitulo1-LOCALIZACIONCA-{{vm.fuenteId}}-{{dat.PoliticaId}}-{{prod.ProductoId}}-{{loc.LocalizacionId}}
                            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-LOCALIZACIONCA-" + vm.fuenteId + "-" + pp.PoliticaId + "-" + p.ProductoId + "-" + l.LocalizacionId);
                            if (campoObligatorioProyectos != undefined) {
                                campoObligatorioProyectos.innerHTML = "";
                                campoObligatorioProyectos.classList.add('hidden');
                            }
                            l.Categorias.forEach(c => {
                                //focalizaciongrcapitulo1-DIMENSIONCA-{{vm.fuenteId}}-{{dat.PoliticaId}}-{{prod.ProductoId}}-{{loc.LocalizacionId}}-{{cat.DimensionId}}
                                var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-DIMENSIONCA-" + + vm.fuenteId + "-" + pp.PoliticaId + "-" + p.ProductoId + "-" + l.LocalizacionId + "-" + c.DimensionId);
                                if (campoObligatorioProyectos != undefined) {
                                    campoObligatorioProyectos.innerHTML = "";
                                    campoObligatorioProyectos.classList.add('hidden');
                                }
                                //focalizaciongrcapitulo1-VIGENCIACA-{{vm.fuenteId}}-{{dat.PoliticaId}}-{{prod.ProductoId}}-{{loc.LocalizacionId}}-{{cat.DimensionId}}-{{vig.Vigencia}}
                                c.Vigencias.forEach(v => {
                                    var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-VIGENCIACA-" + vm.fuenteId + "-" + pp.PoliticaId + "-" + p.ProductoId + "-" + l.LocalizacionId + "-" + c.DimensionId + "-" + v.Vigencia);
                                    if (campoObligatorioProyectos != undefined) {
                                        campoObligatorioProyectos.innerHTML = "";
                                        campoObligatorioProyectos.classList.add('hidden');
                                    }
                                    var campoObligatorioJustificacion = document.getElementById("idinput-" + vm.fuenteId + "-" + pp.PoliticaId + "-" + p.ProductoId + "-" + l.LocalizacionId + "-" + c.DimensionId + "-" + v.Vigencia);
                                    if (campoObligatorioJustificacion != undefined) {
                                        campoObligatorioJustificacion.classList.remove('divInconsistencia');
                                    }


                                })

                            })
                        })
                    })
                }
                );
        }

        vm.errores = {
            'CAT_FUENTE_001': vm.validarCAT_FUENTE_001,

            'CAT_POLITICA_001': vm.validarCAT_POLITICA_001,
            'CAT_PRODUCTO_001': vm.validarCAT_PRODUCTO_001,
            'CAT_LOCALIZACION_001': vm.validarCAT_LOCALIZACION_001,
            'CAT_DIMENSION_001': vm.validarCAT_DIMENSION_001,
            'CAT_VIGENCIA_001': vm.validarCAT_VIGENCIA_001,
            'CAT_GENERAL_001': vm.validarCAT_GENERAL_001,
            'CAT_GENERAL_002': vm.validarCAT_GENERAL_002,

            'ERR_DETALLE_POLITICA_001': vm.validarERR_DETALLE_POLITICA_001,

        }

        /* ------------------------ FIN Validaciones ---------------------------------*/

    }

    var app = angular.module('backbone').component('categoriaProductosPoliticaSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/categoriaProductosPoliticaSgp/categoriaProductosPoliticaSgp.html",        
        controller: categoriaProductosPoliticaSgpController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            callBack: '&',
            valido: '@',
            listaerrores: '@',
            cargado: '@',
        }
    }).directive('stringToNumber', function () {
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
        ;

    app.directive("allowNumbersOnly", function () {
        return {
            restrict: "A",
            link: function (scope, element, attrs) {
                element.bind("keydown", function (event) {
                    /// 8 - (backspace)  46 - (supr)  188 - (,)  190 - (.)
                    if (event.keyCode == 8 || event.keyCode === 46) {
                        return true;
                    } else if (!(event.keyCode > 47 && event.keyCode < 58) || event.shiftKey) {
                        event.preventDefault();
                        return false;
                    }
                });
            }
        }
    });
})();