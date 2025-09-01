(function () {
    'use strict';

    regionalizacionSinTramiteSgpController.$inject = ['$scope', 'recursosAjustesSinTramiteSgpServicio', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'justificacionCambiosServicio'
    ];

    function regionalizacionSinTramiteSgpController(
        $scope,
        recursosAjustesSinTramiteSgpServicio,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        justificacionCambiosServicio
    ) {
        var listaFuentesBase = [];

        var vm = this;
        vm.init = init;
        vm.nombreComponente = "recursossgpregionalizacionsintramitesgp";
        vm.anio = new Date().getFullYear();
        vm.ClasesinputIni = " ";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.actualizarVigencia = actualizarVigencia;
        vm.disabled = true;
        vm.TotalInversionVigencia = 0;
        vm.notificacionErrores = null;
        vm.puntoDigitado = false;
        vm.erroresActivos = [];
        vm.listaSinCofin = [];
        vm.listaConCofin = [];
        vm.ProductoRegionalizadoActual = [];
        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
        vm.habilitaBotones = true;
        var currentYear = new Date().getFullYear();
        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.permiteEditar = false;
        vm.seccionCapitulo = null;
        vm.modelo = null;
        vm.ConvertirNumero = ConvertirNumero;
        vm.abrirMensajeInformacionRegionalizacion = abrirMensajeInformacionRegionalizacion
        vm.valoresEnCero = valoresEnCero;
        vm.actualizaFila = actualizaFila;
        vm.bande = "es";
        vm.Bandera1 = [];
        vm.BanderaTa = "+";
        vm.BanderaTa2 = "+";
        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
        vm.ConvertirNumero4decimales = ConvertirNumero4decimales;
        vm.ObjetivoVerMas = ObjetivoVerMas;
        vm.CurrentDateYear = new Date().getFullYear();
        /*Archivo*/
        vm.nombrearchivo = "";
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.exportExcel = exportExcel;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.ConvertirNumero4 = ConvertirNumero4;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.GuardarArchivo = GuardarArchivo;
        vm.FuenteArchivo = [];
        vm.existeRegionalizacion = false;
        /*Fin Archivo*/
        $sessionStorage.FuenteValidada = null;
        $sessionStorage.ProductoValidado = null;
        vm.abrirMensajeArchivoRegionalizacion = abrirMensajeArchivoRegionalizacion;
        vm.fuenteConsultada = 0;
        vm.productoConsultado = 0;
        vm.localizacionConsultada = 0;
        vm.ErrorProductoDes = [];
        vm.componentesRefresh = [
            'datosgeneralessgphorizontesintramitesgp',
            'datosgeneralessgplocalizacionessintramitesgp',
            'recursossgpfuentesdefinancsintramitesgp',
            'recursossgpcostosdelasactisintramitesgp',
            'datosgeneralessgplocalizacionessintramitesgp',
            'datosgeneralessgpbeneficiariosTotalessintramitesgp'
        ];
        vm.soloLectura = false;

        function abrirMensajeArchivoRegionalizacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Plnatilla Carga Masiva Regionalización, </span><br /> <span class='tituhori'><ul><li>No se permite texto en la columna de 'En ajuste $' y 'Meta en ajuste $'</li><li>La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li><li>La columna 'Meta en ajuste' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li></ul></span>");
        }

        function ObjetivoVerMas(objetivo, titulo) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModalSinTramiteSgp.html',
                controller: 'objetivosIndicadorModalSinTramiteSgpController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return objetivo;
                    },
                    IdObjetivo: function () {
                        return '';
                    },
                    Tipo: function () {
                        return '';
                    },
                    Titulo: function () {
                        return titulo;
                    }
                },
            });
        }

        vm.AbrilNivel1 = function (fuenteId) {
            var variable = $("#ico" + fuenteId)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas" + fuenteId);
            var imgmenos = document.getElementById("imgmenos" + fuenteId);
            if (variable === "+") {
                $("#ico" + fuenteId).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico" + fuenteId).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }
        vm.AbrilNivel2 = function (fuenteId, productoId) {
            var variable = $("#ico" + fuenteId + "-" + productoId)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas" + fuenteId + "-" + productoId);
            var imgmenos = document.getElementById("imgmenos" + fuenteId + "-" + productoId);
            if (variable === "+") {
                $("#ico" + fuenteId + "-" + productoId).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico" + fuenteId + "-" + productoId).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }
        vm.AbrilNivel3 = function (productoId, fuenteId, localizacionId) {

            var variable = $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + productoId + "-" + fuenteId + "-" + localizacionId);
            var imgmenos = document.getElementById("imgmenos-" + productoId + "-" + fuenteId + "-" + localizacionId);
            var detail = $("#detail-" + productoId + "-" + fuenteId + "-" + localizacionId);
            if (variable === "+") {
                $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                if (detail != undefined) detail[0].classList.remove("hidden");

            } else {
                $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                if (detail != undefined) detail[0].classList.add("hidden");
            }

            if (vm.fuenteConsultada != 0 && vm.productoConsultado != 0 && vm.localizacionConsultada != 0) {
                if (vm.fuenteConsultada != fuenteId && vm.productoConsultado != productoId && vm.localizacionConsultada != localizacionId) {
                    var variable = $("#ico" + vm.productoConsultado + "-" + vm.fuenteConsultada + "-" + vm.localizacionConsultada);
                    var imgmas = document.getElementById("imgmas-" + vm.productoConsultado + "-" + vm.fuenteConsultada + "-" + vm.localizacionConsultada);
                    var imgmenos = document.getElementById("imgmenos-" + vm.productoConsultado + "-" + vm.fuenteConsultada + "-" + vm.localizacionConsultada);
                    var detail = $("#detail-" + vm.productoConsultado + "-" + vm.fuenteConsultada + "-" + vm.localizacionConsultada);
                    if (variable === "+") {
                        $("#ico" + vm.productoConsultado + "-" + vm.fuenteConsultada + "-" + vm.localizacionConsultada).html('-');
                        imgmas.style.display = 'none';
                        imgmenos.style.display = 'block';
                        if (detail != undefined) detail[0].classList.remove("hidden");

                    } else {
                        $("#ico" + vm.productoConsultado + "-" + vm.fuenteConsultada + "-" + vm.localizacionConsultada).html('+');
                        imgmas.style.display = 'block';
                        imgmenos.style.display = 'none';
                        if (detail != undefined) detail[0].classList.add("hidden");
                    }
                }
            }

            vm.fuenteConsultada = fuenteId;
            vm.productoConsultado = productoId;
            vm.localizacionConsultada = localizacionId;
        }
        vm.AbrilNivelTabla = function (fila) {
            if (fila == 1) {
                if (vm.BanderaTa == '+') {
                    vm.BanderaTa = '-'
                } else {
                    vm.BanderaTa = '+'
                }
            }
            if (fila == 2) {
                if (vm.BanderaTa2 == '+') {
                    vm.BanderaTa2 = '-'
                } else {
                    vm.BanderaTa2 = '+'
                }
            }
        }

        function HabilitaEditar(band) {

            vm.HabilitaEditarBandera = band;
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
        // ObtenerSeccionCapitulo();-- Solo se llama cuando se guarda el capitulo modificado

        function init() {
            vm.obtenerRegionalizacion(vm.BPIN);
            vm.permiteEditar = false;
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });            
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.esAjuste = $sessionStorage.esAjuste;
        }

        function abrirMensajeInformacionRegionalizacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Proyectos que no se encuentran aún en Ejecución.</span>");
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        vm.refrescarResumenCostos = function () {

            vm.recargaGuardado();
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }
        vm.recargaresumen = function (handler) {
            vm.recargaGuardado = handler;
        }
        vm.recargaresumencostos = function (handler) {
            vm.recargaGuardadoCostos = handler;
        }

        vm.obtenerRegionalizacion = function (bpin) {
            vm.existeRegionalizacion = false;
            var fuentes = [];
            var productos = [];
            var RegionalizadoProducto = [];
            var RegionalizadoFuente = [];
            vm.ProductoRegionalizadoActual = [];
            vm.modelo = null;
            return recursosAjustesSinTramiteSgpServicio.obtenerRegionalizacionGeneralSgp($sessionStorage.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        //vm.modelo = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.modelo = respuesta.data;
                        for (var i = 0; i < vm.modelo.Fuentes.length; i += 1) {
                            if ($sessionStorage.FuenteValidada == undefined || $sessionStorage.FuenteValidada == null) {
                                vm.modelo.Fuentes[i].Muestra = false;
                            }
                            else {
                                fuentes[i] = vm.modelo.Fuentes[i].TotalFuente;
                                RegionalizadoFuente[i] = vm.modelo.Fuentes[i].TotalRegionalizadoFuente;
                                if (fuentes[i] == RegionalizadoFuente[i])
                                    vm.modelo.Fuentes[i].Muestra = false;
                                else
                                    vm.modelo.Fuentes[i].Muestra = true;
                            }
                            for (var j = 0; j < vm.modelo.Fuentes[i].Productos.length; j += 1) {
                                if ($sessionStorage.ProductoValidado == undefined || $sessionStorage.ProductoValidado == null) {
                                    vm.modelo.Fuentes[i].Productos[j].Muestra = false;
                                    vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] = 0;
                                }
                                else {
                                    if (vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] == undefined || vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] == NaN || vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] == null)
                                        vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] = 0;
                                    productos[j] = vm.modelo.Fuentes[i].Productos[j].TotalCostoProducto;
                                    RegionalizadoProducto[j] = vm.modelo.Fuentes[i].Productos[j].TotalRegionalizacionProducto;
                                    vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] = vm.ProductoRegionalizadoActual[vm.modelo.Fuentes[i].Productos[j].ProductoId] + vm.modelo.Fuentes[i].Productos[j].TotalRegionalizacionProducto;
                                    vm.modelo.Fuentes[i].Productos[j].Muestra = true;
                                }

                                for (var k = 0; k < vm.modelo.Fuentes[i].Productos[j].Localizaciones.length; k += 1) {
                                    vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].habilitarFinal = false;
                                    calcularTotales(vm.modelo.Fuentes[i].Productos[j].ProductoId, vm.modelo.Fuentes[i].FuenteId, vm.modelo.Fuentes[i].Productos[j].Localizaciones[k], 0);
                                    for (var l = 0; l < vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias.length; l += 1) {
                                        vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].permiteHabilitar = vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].Vigencia < vm.anio ? false : true;
                                        vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].EnAjusteOriginal = vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].EnAjuste;
                                        vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].MetaEnAjusteOriginal = vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].MetaEnAjuste;
                                        vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].EnAjuste = parseFloat(vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].EnAjuste).toFixed(2);
                                        vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].MetaEnAjuste = parseFloat(vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[l].MetaEnAjuste).toFixed(4);
                                    }
                                }
                            }
                        }
                        vm.existeRegionalizacion = true;
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                }
            );
        }

        function actualizaFila(event, cantidad, vigencias, suma) {

            if (Number.isNaN(event.target.value)) {
                if (cantidad == 2)
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                else
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            if (event.target.value == null) {
                if (cantidad == 2)
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                else
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            if (event.target.value == "") {
                if (cantidad == 2)
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                else
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            event.target.value = procesarNumero(event.target.value);

            if (suma == 1) {
                var acumula = 0;
                angular.forEach(vigencias, function (series) {
                    acumula = acumula + parseFloat(procesarNumero(series.EnAjuste));
                    vigencias.ResumenSubTotal.TotalEnAjuste = parseFloat(acumula).toFixed(cantidad);
                });
            }

            if (suma == 0) {
                var acumula = 0;
                angular.forEach(vigencias, function (series) {
                    acumula = acumula + parseFloat(procesarNumero(series.MetaEnAjuste));
                    vigencias.ResumenSubTotal.TotalMetaEnAjuste = parseFloat(acumula).toFixed(cantidad);
                });
            }

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > cantidad ? event.target.value : parseFloat(val).toFixed(cantidad);
            event.target.value = Intl.NumberFormat('es-co', { minimumFractionDigits: cantidad, }).format(total);
        }

        function procesarNumero(value, cantidadDecimales, convertFloat = true) {
            if (!Number(value)) {
                value = limpiaNumero1(value);

            } else if (!convertFloat) {
                value = value.replace(",", ".");
            } else {
                if (cantidadDecimales != undefined)
                    value = parseFloat(value.replace(",", ".")).toFixed(cantidadDecimales);
            }

            return value;
        }

        function limpiaNumero1(valor) {
            if (`${valor.toLocaleString().split(",")[1]}` == 'undefined') return `${valor.toLocaleString().split(",")[0].toString()}`;
            return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
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

            event.target.value = procesarNumero(event.target.value, null, false);

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");

            if (decimal) {
                if (cantidad == 4) tamanioPermitido = 20;
                else tamanioPermitido = 18;

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
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero, muestraMensaje) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        vm.ActivarEditar = function (producto, fuente, localizacion, muestraMensaje) {
            if (localizacion.habilitarFinal == false) {
                vm.permiteEditar = true;
                localizacion.habilitarFinal = true;
                vm.disabled = false;
                $("#Guardar" + producto + fuente + localizacion.LocalizacionId).attr('disabled', false);
            } else {
                if (muestraMensaje == 1) {
                    utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                        vm.permiteEditar = false;
                        localizacion.habilitarFinal = false;
                        vm.disabled = true;
                        var acumula = 0;
                        angular.forEach(localizacion.Vigencias, function (series) {
                            series.EnAjuste = series.EnAjusteOriginal;
                            series.MetaEnAjuste = series.MetaEnAjusteOriginal;
                            acumula = acumula + parseFloat(series.EnAjusteOriginal);
                            localizacion.Vigencias.ResumenSubTotal.TotalEnAjuste = parseFloat(acumula.toFixed(2));
                        });
                        OkCancelar();
                        $("#Guardar" + producto + fuente + localizacion.LocalizacionId).attr('disabled', true);

                    }, function funcionCancelar(reason) {
                        return;
                    }, null, null, "Los posibles datos que haya diligenciado en la tabla 'Detalle Localización' se perderán");
                }
                else {
                    vm.permiteEditar = false;
                    localizacion.habilitarFinal = false;
                    vm.disabled = true;
                    $("#Guardar" + producto + fuente + localizacion.LocalizacionId).attr('disabled', true);
                }


            }
        }

        function OkCancelar() {
            vm.activarControles('inicio');
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
            }, 500);
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
                // SeccionCapituloId: seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: true,
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    console.log(response);
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function valoresEnCero(ProductoId, fuenteId, localizacion) {
            utilidades.mensajeWarning("Los valores de las columnas 'En Ajuste$' y 'Meta en ajuste' quedarán en ceros, ¿desea continuar?", function funcionContinuar() {

                localizacion.Vigencias.forEach(vi => {
                    vi.MetaEnAjuste = 0;
                    vi.EnAjuste = 0;
                    vi.MetaEnFirme = 0;
                    vi.EnFirme = 0;
                    var valoresarchivo = {
                        Bpin: vm.modelo.BPIN,
                        ProductoId: ProductoId,
                        FuenteId: fuenteId,
                        LocalizacionId: localizacion.LocalizacionId,
                        Vigencia: vi.Vigencia,
                        PeriodoProyectoId: vi.PeriodoProyectoId,
                        TotalFuene: 0,
                        TotalRegionalizadoFuente: 0,
                        TotalCostoProducto: 0,
                        TotalRegionalizadoProducto: 0,
                        EnAjuste: 0,
                        MetaEnAjuste: 0,
                    };
                    vm.FuenteArchivo.push(valoresarchivo);
                    calcularTotales(ProductoId, fuenteId, localizacion, 0);
                });
                GuardarArchivo(true, 0);


            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            });

        }

        function actualizarVigencia(producto, fuente, localizacion, afectaBoton, item) {
            var arrayProducto = item.Productos.find(e => e.ProductoId == producto);
            var TotalEnFirme = 0;
            var TotalEnAjuste = 0;
            var TotalMetaEnFirme = 0;
            var TotalMetaEnAjuste = 0;
            localizacion.Vigencias.forEach(ff => {
                TotalEnFirme = parseFloat(TotalEnFirme) + parseFloat(ff.EnFirme);
                TotalEnAjuste = parseFloat(TotalEnAjuste) + parseFloat(ff.EnAjuste);
                TotalMetaEnFirme = parseFloat(TotalMetaEnFirme) + parseFloat(ff.MetaEnFirme);
                TotalMetaEnAjuste = parseFloat(TotalMetaEnAjuste) + parseFloat(ff.MetaEnAjuste);
            });

            localizacion.Vigencias.ResumenSubTotal = {
                icono: "=",
                tituloOperacion: "Total $",
                TotalEnFirme: TotalEnFirme,
                TotalEnAjuste: TotalEnAjuste,
                TotalMetaEnFirme: TotalMetaEnFirme,
                TotalMetaEnAjuste: TotalMetaEnAjuste,
            }
            if (afectaBoton == 1)
                vm.ActivarEditar(producto, fuente, localizacion, 0);
            localizacion.Vigencias.forEach(vi => {
                var valoresarchivo = {
                    Bpin: vm.modelo.BPIN,
                    ProductoId: producto,
                    FuenteId: fuente,
                    LocalizacionId: localizacion.LocalizacionId,
                    Vigencia: vi.Vigencia,
                    PeriodoProyectoId: vi.PeriodoProyectoId,
                    TotalFuene: item.TotalFuente,
                    TotalRegionalizadoFuente: item.TotalRegionalizadoFuente,
                    TotalCostoProducto: arrayProducto.TotalCostoProducto,
                    TotalRegionalizadoProducto: arrayProducto.TotalRegionalizacionProducto,
                    EnAjuste: vi.EnAjuste,
                    MetaEnAjuste: vi.MetaEnAjuste,
                };
                vm.FuenteArchivo.push(valoresarchivo);
            });
            $sessionStorage.FuenteValidada = fuente;
            $sessionStorage.ProductoValidado = producto;
            GuardarArchivo(false, 0);
        }

        function calcularTotales(producto, fuente, localizacion, afectaBoton) {
            var TotalEnFirme = 0;
            var TotalEnAjuste = 0;
            var TotalMetaEnFirme = 0;
            var TotalMetaEnAjuste = 0;
            localizacion.Vigencias.forEach(ff => {
                TotalEnFirme = parseFloat(TotalEnFirme) + parseFloat(ff.EnFirme);
                TotalEnAjuste = parseFloat(TotalEnAjuste) + parseFloat(ff.EnAjuste);
                TotalMetaEnFirme = parseFloat(TotalMetaEnFirme) + parseFloat(ff.MetaEnFirme);
                TotalMetaEnAjuste = parseFloat(TotalMetaEnAjuste) + parseFloat(ff.MetaEnAjuste);
            });

            localizacion.Vigencias.ResumenSubTotal = {
                icono: "=",
                tituloOperacion: "Total $",
                TotalEnFirme: TotalEnFirme,
                TotalEnAjuste: TotalEnAjuste,
                TotalMetaEnFirme: TotalMetaEnFirme,
                TotalMetaEnAjuste: TotalMetaEnAjuste,
            }
            if (afectaBoton == 1)
                vm.ActivarEditar(producto, fuente, localizacion, 0);

        }

        vm.validateFormat = function (event, cantidad) {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44 && event.keyCode != 46) {
                event.preventDefault();
            }

        }

        function limpiaNumero(valor) {
            return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
        }

        function validarEntero(valor) {
            //intento convertir a entero. 
            //si era un entero no le afecta, si no lo era lo intenta convertir 
            valor = parseInt(limpiaNumero(valor))

            //Compruebo si es un valor numérico 
            if (isNaN(valor)) {
                //entonces (no es numero) devuelvo el valor cadena vacia 
                return ""
            } else {
                //En caso contrario (Si era un número) devuelvo el valor 
                return valor
            }
        }



        /*Archivo*/
        vm.activarControles = function (evento) {
            switch (evento) {
                case "inicio":
                    $("#btnRegionalizacionValidarArchivo").attr('disabled', true);
                    $("#btnRegionalizacionLimpiarArchivo").attr('disabled', true);
                    $("#btnRegionalizacionArchivoSeleccionado").attr('disabled', true);
                    document.getElementById('file').value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnRegionalizacionValidarArchivo").attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo").attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnRegionalizacionValidarArchivo").attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo").attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado").attr('disabled', false);
                    break;
                default:
            }
        }

        function exportExcel() {
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
                        name: 'Bpin', title: 'Bpin'
                    },
                    {
                        name: 'FuenteId', title: 'Fuente Id'
                    },
                    {
                        name: 'ProductoId', title: 'Producto Id'
                    },
                    {
                        name: 'LocalizacionId ', title: 'Localizacion Id'
                    },
                    {
                        name: 'PeriodoProyectoId ', title: 'PeriodoProyecto Id'
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
                        name: 'TotalRegionalizadoFuente', title: 'Total Reionalizado Fuente'
                    },
                    {
                        name: 'Producto', title: 'Producto'
                    },
                    {
                        name: 'TotalCostoProducto', title: 'Total Costo Producto'
                    },
                    {
                        name: 'TotalRegionalizacionProducto', title: 'Total Regionalizacion Producto'
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
                        name: 'MetaEnAjuste', title: 'Meta En Ajuste'
                    },
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla Ajuste Regionalizacion",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Hoja Plantilla");

                const header = colNames;
                const data = [];

                //vm.Fuentes.Fuentes.forEach(fuente => {
                vm.modelo.Fuentes.forEach(fuente => {
                    fuente.Productos.forEach(producto => {
                        producto.Localizaciones.forEach(localizacion => {
                            var localiza = "";
                            if (localizacion.Municipio == null) {
                                localiza = localizacion.Departamento;
                            }
                            else {
                                localiza = localizacion.Departamento + " - " + localizacion.Municipio;
                            }

                            localizacion.Vigencias.forEach(vigencia => {
                                data.push({
                                    Bpin: vm.modelo.BPIN,
                                    FuenteId: fuente.FuenteId,
                                    ProductoId: producto.ProductoId,
                                    LocalizacionId: localizacion.LocalizacionId,
                                    PeriodoProyectoId: vigencia.PeriodoProyectoId,
                                    Fuente: fuente.TipoFinanciador + " " + fuente.Financiador + " " + fuente.Recurso,
                                    Etapa: fuente.Etapa,
                                    TipoFinanciador: fuente.TipoFinanciador,
                                    Financiador: fuente.Financiador,
                                    Recurso: fuente.Recurso,
                                    TotalFuente: ConvertirNumero2decimales(fuente.TotalFuente),
                                    TotalRegionalizadoFuente: ConvertirNumero2decimales(fuente.TotalRegionalizadoFuente),
                                    Producto: producto.Producto,
                                    TotalCostoProducto: ConvertirNumero2decimales(producto.TotalCostoProducto),
                                    TotalRegionalizacionProducto: ConvertirNumero2decimales(producto.TotalRegionalizacionProducto),
                                    Localizacion: localiza,
                                    Vigencia: vigencia.Vigencia,
                                    EnAjuste: ConvertirNumero2decimales(vigencia.EnAjuste),// parseFloat(vigencia.EnAjuste),//.replaceAll('.', ','),
                                    MetaEnAjuste: ConvertirNumero4decimales(vigencia.MetaEnAjuste)  //parseFloat(vigencia.MetaEnAjuste),//.replaceAll('.', ','),
                                });
                            });
                        });
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: ["Bpin", "FuenteId", "ProductoId", "LocalizacionId", "PeriodoProyectoId", "Fuente", "Etapa", "TipoFinanciador", "Financiador", "Recurso", "TotalFuente",
                        "TotalRegionalizadoFuente", "Producto", "TotalCostoProducto", "TotalRegionalizacionProducto", "Localizacion", "Vigencia", "EnAjuste", "MetaEnAjuste"]
                });

                /* hide second column */
                worksheet['!cols'] = [];
                //worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };
                worksheet['!cols'][4] = { hidden: true };

                wb.Sheets["Hoja Plantilla"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });                
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaAjusteRegionalizacion.xlsx');
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function formatColumn(worksheet, col) {
            var fmtnumero2 = "#.##0,00";// "##,##";
            var fmtnumero4 = "#.##0,0000";//"#,####";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })
                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0" /*|| ref != "R0" || ref != "S0"*/) {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "R1") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "S1") {
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

        function limpiarArchivo() {
            $scope.files = [];
            document.getElementById('file').value = "";
            vm.activarControles('inicio');
            vm.nombrearchivo = "";
        }

        function validarArchivo() {
            var resultado = true;
            var enajuste = 0;
            var metaEnAjuste = 0;
            vm.FuenteArchivo = [];
            if (file.files.length > 0) {

                let file = document.getElementById("file").files[0];
                if ($scope.validaNombreArchivo(file.name)) {
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

                                    if (item["PeriodoProyectoId"] == undefined) {
                                        utilidades.mensajeError("La columna PeriodoProyectoId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["PeriodoProyectoId"])) {
                                        utilidades.mensajeError("El valor PeriodoProyectoId " + item["PeriodoProyectoId"] + " no es númerico!");
                                        return false;
                                    }


                                    if (item["EnAjuste"] == undefined) {
                                        utilidades.mensajeError("La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(procesarNumero(item["EnAjuste"].toString()), 2)) {
                                        utilidades.mensajeError("Valor no valido 'En ajuste $' " + item["EnAjuste"] + ". La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        enajuste = item["EnAjuste"];
                                    }

                                    if (item["MetaEnAjuste"] == undefined) {
                                        utilidades.mensajeError("La columna 'Meta En Ajuste' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(procesarNumero(item["MetaEnAjuste"].toString()), 4)) {
                                        utilidades.mensajeError("Valor no valido 'Meta En Ajuste' " + item["MetaEnAjuste"] + ". La columna 'Meta En Ajuste $' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        metaEnAjuste = item["MetaEnAjuste"];
                                    }



                                    var valoresarchivo = {
                                        Bpin: item["Bpin"],
                                        ProductoId: item["ProductoId"],
                                        FuenteId: item["FuenteId"],
                                        LocalizacionId: item["LocalizacionId"],
                                        Vigencia: item["Vigencia"],
                                        PeriodoProyectoId: item["PeriodoProyectoId"],

                                        TotalFuene: item["TotalFuente"],
                                        TotalRegionalizadoFuente: item["TotalRegionalizadoFuente"],
                                        TotalCostoProducto: item["TotalCostoProducto"],
                                        TotalRegionalizadoProducto: item["TotalRegionalizadoProducto"],

                                        EnAjuste: enajuste,// item["EnAjuste"],
                                        MetaEnAjuste: metaEnAjuste,//item["MetaEnAjuste"],
                                    };
                                    vm.FuenteArchivo.push(valoresarchivo);

                                });

                                if (resultado.indexOf(false) == -1) {
                                    if (!ValidarRegistros()) {
                                        utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
                                        vm.activarControles('inicio');
                                    }
                                    else {
                                        vm.activarControles('validado');
                                        utilidades.mensajeSuccess("Proceda a cargar el archivo para que quede registrado en el sistema", false, false, false, "Validación de carga exitosa.");
                                    }
                                }
                                else {
                                    vm.activarControles('inicio');
                                    vm.FuenteArchivo = [];
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
        }

        function ValidarRegistros() {
            var aFuentes = [];
            var aProductos = [];
            var aLocalizaciones = [];
            var aPeriodoProyecto = [];
            var aVigencias = [];

            var existeBpin = 0;
            var existeFuente = 0;
            var existeProducto = 0;
            var existeLocaliacion = 0;
            var existeVigencia = 0;
            var existePeriodoProyecto = 0;
            var CantidadRegistros = 0;

            vm.modelo.Fuentes.forEach(fuente => {
                aFuentes.push(fuente.FuenteId);
                fuente.Productos.forEach(producto => {
                    aProductos.push(producto.ProductoId);
                    producto.Localizaciones.forEach(localizacion => {
                        aLocalizaciones.push(localizacion.LocalizacionId);
                        localizacion.Vigencias.forEach(vigencia => {
                            aPeriodoProyecto.push(vigencia.PeriodoProyectoId);
                            aVigencias.push(vigencia.Vigencia);
                            CantidadRegistros = CantidadRegistros + 1;
                        });
                    });
                });
            });

            vm.FuenteArchivo.forEach(fa => {

                if (fa.Bpin != vm.modelo.BPIN) {
                    existeBpin++;
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
                if (aPeriodoProyecto.indexOf(fa.PeriodoProyectoId) == -1) {
                    existePeriodoProyecto = existePeriodoProyecto + 1;
                }
                if (aVigencias.indexOf(fa.Vigencia) == -1) {
                    existeVigencia = existeVigencia + 1;
                }
            });

            if (existeBpin > 0 || existeFuente > 0 || existeProducto > 0 || existeLocaliacion > 0 || existeVigencia > 0 || existePeriodoProyecto > 0) {
                return false;
            }
            else {
                if (CantidadRegistros != vm.FuenteArchivo.length) {
                    return false;
                }
                else {
                    return true;
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

        function ConvertirNumero4(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        $scope.validaNombreArchivo = function (nombre) {
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

        $scope.fileNameChanged = function (input) {
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                vm.activarControles('cargaarchivo');
            }
            else {
                vm.filename = input.files.length + " archivos"
                vm.activarControles('inicio');
            }
        }

        $scope.ChangeSet = function () {
            if (vm.nombrearchivo == "") {
                vm.activarControles('inicio');
            }
        };

        function adjuntarArchivo() {
            document.getElementById('file').value = "";
            document.getElementById('file').click();
        }

        function GuardarArchivo(valorescero, cargamasiva) {
            if (cargamasiva)
                vm.activarControles('inicio');
            return recursosAjustesSinTramiteSgpServicio.guardarRegionalizacionFuentes(vm.FuenteArchivo, vm.idUsuario).then(function (response) {

                if (response.statusText === "OK" || response.status === 200) {
                    if (valorescero == true) {
                        vm.modelo.Fuentes.forEach(fuente => {
                            if (fuente.FuenteId == vm.FuenteArchivo[0].FuenteId) {
                                fuente.Productos.forEach(producto => {
                                    if (producto.ProductoId == vm.FuenteArchivo[0].ProductoId) {
                                        producto.Localizaciones.forEach(localizacion => {
                                            calcularTotales(vm.FuenteArchivo[0].ProductoId, vm.FuenteArchivo[0].FuenteId, localizacion, 0);
                                            if (localizacion.LocalizacionId == vm.FuenteArchivo[0].LocalizacionId) {
                                                localizacion.Vigencias.forEach(vigencia => {
                                                    vigencia.EnAjuste = 0;
                                                    vigencia.MetaEnAjuste = 0;
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                        });
                        vm.FuenteArchivo = [];
                    }
                    else {
                        vm.obtenerRegionalizacion(vm.modelo.BPIN);
                    }
                    guardarCapituloModificado();
                    utilidades.mensajeSuccess("Verifique ahora el total del producto. Si este presenta inconsistencias, ajuste la sumatoria de los costos regionalizados en las localizaciones por producto (columna 'En ajuste'), hasta que coincidan.", false, false, false, "Los datos fueron guardados con éxito");

                    vm.FuenteArchivo = [];
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
        /*Fin Archivo*/


        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined && errores.length > 0) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);
                isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var Error = p.Error.split('-')[0];
                        var FuenteId = p.Error.split('-')[1];
                        if (vm.errores[Error] != undefined) {
                            switch (Error) {
                                case "ErrorFuente":
                                    vm.ErrorFuente(FuenteId, p.Descripcion);
                                    break;
                                case "ErrorProducto":
                                    var ProductoId = p.Error.split('-')[2];
                                    vm.ErrorProducto(FuenteId, ProductoId, p.Descripcion);
                                    break;
                                default:
                                    vm.limpiarErrores();
                            }
                        }                            
                    });
                }
            }
            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        };

        vm.limpiarErrores = function () {
            if (vm.modelo != null) {
                if (vm.modelo.Fuentes != undefined) {
                for (var i = 0; i < vm.modelo.Fuentes.length; i += 1) {
                    vm.modelo.Fuentes[i].Muestra = false;
                    for (var j = 0; j < vm.modelo.Fuentes[i].Productos.length; j += 1) {
                        vm.modelo.Fuentes[i].Productos[j].Muestra = false;
                        for (var k = 0; k < vm.modelo.Fuentes[i].Productos[j].Localizaciones.length; k += 1) {
                            for (var m = 0; m < vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias.length; m += 1) {
                                if (vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[m].EnAjuste > 0 || (vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[m].EnAjuste == 0 && vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[m].MetaEnAjuste == 0)) {
                                    var localizacionIconError = document.getElementById("localizacionIconError-" + vm.modelo.Fuentes[i].FuenteId + "-" + vm.modelo.Fuentes[i].Productos[j].ProductoId + "-" + vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].LocalizacionId + "-" + vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].Vigencias[m].Vigencia);
                                    if (localizacionIconError != undefined) {
                                        localizacionIconError.classList.add('hidden');
                                    }
                                    var localizacionError = document.getElementById("localizacionError-" + vm.modelo.Fuentes[i].FuenteId + "-" + vm.modelo.Fuentes[i].Productos[j].ProductoId + "-" + vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].LocalizacionId);
                                    if (localizacionError != undefined) {
                                        localizacionError.classList.add('hidden');
                                    }
                                    var localizacionFooterError = document.getElementById("localizacionFooterError-" + vm.modelo.Fuentes[i].FuenteId + "-" + vm.modelo.Fuentes[i].Productos[j].ProductoId + "-" + vm.modelo.Fuentes[i].Productos[j].Localizaciones[k].LocalizacionId);
                                    if (localizacionFooterError != undefined) {
                                        localizacionFooterError.classList.add('hidden');
                                    }
                                    var RegionalizacionLocalizacionIconError = document.getElementById("RegionalizacionLocalizacionIconError-" + vm.modelo.Fuentes[i].FuenteId + "-" + vm.modelo.Fuentes[i].Productos[j].ProductoId);
                                    if (RegionalizacionLocalizacionIconError != undefined) {
                                        RegionalizacionLocalizacionIconError.classList.add('hidden');
                                    }
                                    var RegionalizacionFuenteIconError = document.getElementById("RegionalizacionFuenteIconError-" + vm.modelo.Fuentes[i].FuenteId);
                                    if (RegionalizacionFuenteIconError != undefined) {
                                        RegionalizacionFuenteIconError.classList.add('hidden');
                                    }
                                }
                            }

                        }
                    }
                    }
                }
            }

            if (vm.modelo != null) {
                if (vm.modelo.Fuentes != undefined) {
                    vm.modelo.Fuentes.forEach(f => {
                        var ErrorFuente = document.getElementById("ErrorFuente-" + f.FuenteId);
                        var ErrorFuentemsn = document.getElementById("ErrorFuenteErrorMsg-" + f.FuenteId);
                        if (ErrorFuente != undefined) {
                            if (ErrorFuentemsn != undefined) {
                                ErrorFuentemsn.innerHTML = '';
                                ErrorFuente.classList.add('hidden');
                            }
                        }
                        f.Productos.forEach(p => {
                            var ErrorProducto = document.getElementById("ErrorProducto-" + f.FuenteId + "-" + p.ProductoId);
                            var ErrorProductomsn = document.getElementById("ErrorProductoErrorMsg-" + f.FuenteId + "-" + p.ProductoId);
                            if (ErrorProducto != undefined) {
                                if (ErrorProductomsn != undefined) {
                                    ErrorProductomsn.innerHTML = '';
                                    ErrorProducto.classList.add('hidden');
                                }
                            }
                        });

                    });
                }
            }
            vm.ErrorProductoDes = [];
        }

        vm.ErrorFuente = function (FuenteId, descripcion) {
            var ErrorFuente = document.getElementById("ErrorFuente-" + FuenteId);
            var ErrorFuentemsn = document.getElementById("ErrorFuenteErrorMsg-" + FuenteId);

            if (ErrorFuente != undefined) {
                if (ErrorFuentemsn != undefined) {
                    ErrorFuentemsn.innerHTML = '<span>' + descripcion + "</span>";
                    ErrorFuente.classList.remove('hidden');
                }
            }
        }

        vm.ErrorProducto = function (FuenteId, ProductoId, descripcion) {
            var ErrorProducto = document.getElementById("ErrorProducto-" + FuenteId + "-" + ProductoId);
            var ErrorProductomsn = document.getElementById("ErrorProductoErrorMsg-" + FuenteId + "-" + ProductoId);

            if (ErrorProducto != undefined) {
                if (ErrorProductomsn != undefined) {
                   ErrorProductomsn.innerHTML = '<span>' + descripcion + "</span>";
                    ErrorProducto.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'ErrorFuente': vm.ErrorFuente,
            'ErrorProducto': vm.ErrorProducto,
            //'FUE001': vm.validarExitenciaFuentes;
            '': vm.limpiarErrores()
        }
    }

    angular.module('backbone').component('regionalizacionSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/regionalizacion/regionalizacionSinTramiteSgp.html",
        controller: regionalizacionSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&'
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