(function () {
    'use strict';

    focalizacionPoliticasController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        '$window',
        'utilidades',
        'desagregarEdtServicio',
        'indicadorPoliticasServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'focalizacionPoliticasServicio'
    ];

    function focalizacionPoliticasController(
        $scope,
        $sessionStorage,
        $uibModal,
        $window,
        utilidades,
        desagregarEdtServicio,
        indicadorPoliticasServicio,
        utilsValidacionSeccionCapitulosServicio,
        focalizacionPoliticasServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "segfocalizacionsegfocpoliticas";

        vm.listadoIndicadoresPolitica = [];
        vm.unidadesMedida = [];
        vm.periodos = [];
        vm.FocalizacionProgramacionSeguimiento = "";
        vm.FocalizacionProgramacionSeguimientoorigen = "";
        vm.FocalizacionProgramacionSeguimientoDetalle = "";
        vm.FocalizacionProgramacionSeguimientoDetalleorigen = "";
        vm.DetallePolitica = "";
        vm.currentYear = new Date().getFullYear();
        vm.Editar = false;
        vm.tr = [];
        vm.col = 4;
        vm.ProductoCategoriaGuardar = "";
        vm.refreshfocalizacion = 'false';
        vm.VerSubCtg = 3;
        vm.MensajeCtg = "";
        vm.MensajeCtgcorto = "";
        vm.Errorres = [];
        vm.pintartablas = 0;
        vm.PolitcaSeleccionada = 0;
        vm.FuenteConsultadaId = 0;
        vm.DimensionConsultadaId = 0;
        vm.ObjetivoConsultadoId = 0;
        vm.ProductoConsultadoId = 0;
        vm.LocalizacionConsutadaId = 0;
        vm.FuenteId = null;


        vm.ConsultarDetallePolitica = ConsultarDetallePolitica;
        vm.abrirMensajeDetallePolitica = abrirMensajeDetallePolitica;
        vm.abrirMensajeremebe = abrirMensajeremebe;
        vm.abrirMensajeresumenavafoc = abrirMensajeresumenavafoc;
        vm.exportExcel = exportExcel;
        vm.modelo = null;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.nombrearchivo = "";
        vm.validarArchivof = validarArchivof;
        vm.limpiarArchivo = limpiarArchivo;
        vm.GuardarArchivof = GuardarArchivo;
        vm.abrirMensajeArchivoRegionalizacion = abrirMensajeArchivoRegionalizacion;
        vm.soloLectura = $sessionStorage.soloLectura;

        vm.componentesRefresh = [
            "segfocalizacionsegcrucepoliticas"
        ];


        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificarrefresco({ handler: vm.notificarRefrescoFuentes, nombreComponente: vm.nombreComponente });
            vm.refreshComponente();
            vm.obtenerPeriodos();
        };

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'segfocalizacionsegcrucepoliticas': true
            };
        }

        vm.refreshComponente = function () {
            vm.ObtenerFocalizacionProgramacionSeguimiento();
            var arr = []
            localStorage.setItem('ErroresRAFOC', JSON.stringify(arr));
            this.limpiarErrores
        }

        vm.reloadEdicion = function (dataSeleccionada) {
            vm.abrirArbol(dataSeleccionada.DataAgregarModal);
            vm.guardadoevent({ nombreComponenteHijo: this.nombreComponente })
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        vm.ObtenerFocalizacionProgramacionSeguimiento = function () {
            vm.FocalizacionProgramacionSeguimiento = "";
            vm.FocalizacionProgramacionSeguimientoorigen = "";
            vm.FocalizacionProgramacionSeguimientoDetalle = "";
            vm.FocalizacionProgramacionSeguimientoDetalleorigen = "";
            vm.modelo = null;
            const proyecto = {
                InstanciaId: $sessionStorage.idInstancia,
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Texto: "",
                DetalleLocalizacion: [{
                    FuenteId: null,
                    PoliticaId: null,
                }]
            };
            var parametroConsulta = JSON.stringify(proyecto);

            focalizacionPoliticasServicio.ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta).then(resultado => {
                if (resultado.data != null) {
                    var arreglolistas = jQuery.parseJSON(resultado.data);
                    vm.FocalizacionProgramacionSeguimiento = jQuery.parseJSON(arreglolistas);
                    vm.FocalizacionProgramacionSeguimientoorigen = jQuery.parseJSON(arreglolistas);
                    vm.modelo = jQuery.parseJSON(arreglolistas);

                    vm.DetallePolitica = [];
                    if (vm.FocalizacionProgramacionSeguimiento != null && vm.FocalizacionProgramacionSeguimiento.DetallePoliticas != null) {
                        ConsultarDetallePolitica(vm.FocalizacionProgramacionSeguimiento.DetallePoliticas[0].PoliticaId)
                        if (vm.pintartablas == 0) {
                            CalcularTablaPoliticas();
                            vm.pintartablas = 1;
                        }
                    }

                    if (vm.FocalizacionProgramacionSeguimiento.ListaPeriodosActivos == null) {
                        vm.HabilitaCargue = true;
                        $scope.isBlue = false;
                        $scope.isGrey = true;
                    } else {
                        vm.HabilitaCargue = false;
                        $scope.isBlue = true;
                        $scope.isGrey = false;
                    }
                }
            });
        }

        function CalcularTablaPoliticas() {
            vm.tr = [];
            var j = 0;
            var k = 0;
            for (var i = 0; i < vm.FocalizacionProgramacionSeguimiento.ListaPoliticas.length; i++) {

                if (j <= 2) {
                    if (j == 0) {
                        var adicion = {
                            id: k,
                            td: [{
                                PoliticaId: vm.FocalizacionProgramacionSeguimiento.ListaPoliticas[i].PoliticaId,
                                NombrePolitica: vm.FocalizacionProgramacionSeguimiento.ListaPoliticas[i].NombrePolitica
                            }]
                        };
                        vm.tr.push(adicion)
                        j++;
                    }
                    else {
                        var datos = {
                            PoliticaId: vm.FocalizacionProgramacionSeguimiento.ListaPoliticas[i].PoliticaId,
                            NombrePolitica: vm.FocalizacionProgramacionSeguimiento.ListaPoliticas[i].NombrePolitica
                        };
                        vm.tr[k].td.push(datos);
                        j++;
                    }
                }
                else {
                    j = 1;
                    k++;

                    var adicion = {
                        id: k,
                        td: [{
                            PoliticaId: vm.FocalizacionProgramacionSeguimiento.ListaPoliticas[i].PoliticaId,
                            NombrePolitica: vm.FocalizacionProgramacionSeguimiento.ListaPoliticas[i].NombrePolitica
                        }]
                    };
                    vm.tr.push(adicion)
                }
            }
        }
        function abrirMensajeDetallePolitica() {
            utilidades.mensajeInformacionN("", null, null, "<span class='tituhori' > Mensaje Detalle Politica.</span>");
        }

        function abrirMensajeremebe() {
            utilidades.mensajeInformacionN("", null, null, "<span class='tituhori' > Mensaje Recursos metas y beneficiarios.</span>");
        }

        function abrirMensajeresumenavafoc() {
            utilidades.mensajeInformacionN("", null, null, "<span class='tituhori' > Mensaje Resumen avance focalización.</span>");
        }

        function ConsultarDetallePolitica(politicaid) {
            vm.PolitcaSeleccionada = politicaid;
            vm.DetallePolitica = [];
            if (vm.FocalizacionProgramacionSeguimiento != null && vm.FocalizacionProgramacionSeguimiento.DetallePoliticas != null) {
                vm.FocalizacionProgramacionSeguimiento.DetallePoliticas.forEach(detalle => {
                    if (detalle.PoliticaId == politicaid) {
                        vm.DetallePolitica = detalle;
                    }
                });

                var vigentes = 0;
                var compromisos = 0;
                var obligaciones = 0;
                var pagos = 0;

                vm.DetallePolitica.Fuentes.forEach(fuente => {
                    fuente.Categorias.forEach(cat => {
                        cat.NombreCategoriaCorto = cat.NombreSubCategoria.substring(0, 140);
                        cat.Objetivos.forEach(obj => {
                            obj.Productos.forEach(pro => {
                            });

                        });
                    });
                });
            }
            setTimeout(function () {
                PintarValidaciones();
            }, 500);
        }

        function PintarValidaciones() {
            if (JSON.parse(localStorage.getItem('ErroresRAFOC')) != null) {
                var erroresvalidar = JSON.parse(localStorage.getItem('ErroresRAFOC'));
                if (erroresvalidar.length > 0) {
                    var erroresActivos = [];
                    vm.limpiarErrores();
                    vm.PintarErrorGenerales();
                    erroresvalidar.forEach(error => {
                        var valor = 0;

                        var erroresActivos = {
                            Error: error.Error,
                            descripcion: error.Descripcion,
                            Data: error.Data
                        };
                        var valorw = 0;
                        switch (error.Error) {
                            case "AVFOG001":
                                vm.validarAVFOG001(erroresActivos);
                                break;
                            case "AVFOG002":
                                vm.validarAVFOG002(erroresActivos);
                                break;
                            case "AVFOG003":
                                vm.validarAVFOG003(erroresActivos);
                                break;
                            case "AVFOG004":
                                vm.validarAVFOG004(erroresActivos);
                                break;
                            case "AVFOG005":
                                vm.validarAVFOG005(erroresActivos);
                                break;
                            case "AVFOG006":
                                vm.validarAVFOG006(erroresActivos);
                                break;
                            case "AVFOG007":
                                vm.validarAVFOG007(erroresActivos);
                                break;
                            default:
                                break;
                        }
                    });                    
                }
            }
        }

        vm.obtenerPeriodos = function () {
            vm.periodos = [];
            indicadorPoliticasServicio.obtenerCalendarioPeriodo({ bpin: $sessionStorage.bpinProductos })
                .then(resultado => {
                    console.log(resultado);
                    vm.periodos = resultado.data;
                });
        }

        vm.obtenerParametricas = function () {
            desagregarEdtServicio.obtenerUnidadesMedida()
                .then(resultado => {
                    if (resultado != null) vm.unidadesMedida = resultado.data;
                })
        }

        /*--------------------- Comportamientos collapse y contenido ---------------------*/

        vm.AbrilNivel = function (fuenteid, dimensionId, indexcategoria) {

            var variable = $("#icoreavfocpol-" + fuenteid + "-" + dimensionId + "-" + indexcategoria)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasreavfocpol-" + fuenteid + "-" + dimensionId + "-" + indexcategoria);
            var imgmenos = document.getElementById("imgmenosreavfocpol-" + fuenteid + "-" + dimensionId + "-" + indexcategoria);
            if (variable === "+") {
                $("#icoreavfocpol-" + fuenteid + "-" + dimensionId + "-" + indexcategoria).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icoreavfocpol-" + fuenteid + "-" + dimensionId + "-" + indexcategoria).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.FuenteConsultadaId = fuenteid;
            vm.DimensionConsultadaId = dimensionId;

            setTimeout(function () {
                PintarValidaciones();
            }, 500);

        }

        vm.AbrilNivelProducto = function (fuenteid, dimensionId, indexcategoria, objetivoId, indexobjetivo, productoId, indexproducto) {

            var variable = $("#icoreavfocpolproducto-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasreavfocpolproducto-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto);
            var imgmenos = document.getElementById("imgmenosreavfocpolproducto-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto);
            if (variable === "+") {
                $("#icoreavfocpolproducto-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icoreavfocpolproducto-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.FuenteConsultadaId = fuenteid;
            vm.DimensionConsultadaId = dimensionId;
            vm.ObjetivoConsultadoId = objetivoId;
            vm.ProductoConsultadoId = productoId;

            setTimeout(function () {
                PintarValidaciones();
            }, 500);

        }

        vm.AbrirNivelLocalizacion = function (fuenteid, dimensionId, indexcategoria, objetivoId, indexobjetivo, productoId, indexproducto, LocalizacionId, indexlocalizacion) {

            var variable = $("#icoreavfocloc-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasreavfocloc-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion);
            var imgmenos = document.getElementById("imgmenosreavfocloc-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion);
            if (variable === "+") {
                $("#icoreavfocloc-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icoreavfocloc-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.FuenteConsultadaId = fuenteid;
            vm.DimensionConsultadaId = dimensionId;
            vm.ObjetivoConsultadoId = objetivoId;
            vm.ProductoConsultadoId = productoId;
            vm.LocalizacionConsutadaId = LocalizacionId;

            CargarDetalleLocalizacion(fuenteid, productoId, LocalizacionId, dimensionId);

            setTimeout(function () {
                PintarValidaciones();
            }, 500);
        }

        function CargarDetalleLocalizacion(fuenteid, productoId, LocalizacionId, dimensionId) {
            var valor = 0;
            const proyecto = {
                InstanciaId: $sessionStorage.idInstancia,
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                DetalleLocalizacion: [{
                    FuenteId: fuenteid,
                    PoliticaId: vm.DetallePolitica.PoliticaId,
                    ProductoId: productoId,
                    LocalizacionId: LocalizacionId,
                    DimensionId: dimensionId
                }]
            };

            vm.FocalizacionProgramacionSeguimientoDetalle = "";
            vm.FocalizacionProgramacionSeguimientoDetalleorigen = "";

            var parametroConsulta = JSON.stringify(proyecto);
            return focalizacionPoliticasServicio.ObtenerFocalizacionProgramacionSeguimientoDetalle(parametroConsulta, usuarioDNP).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.FocalizacionProgramacionSeguimientoDetalle = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                });
        }

        vm.AbrirNivelLocalizacionResumenRecursos = function (fuenteid, dimensionId, indexcategoria, objetivoId, indexobjetivo, productoId, indexproducto, LocalizacionId, indexlocalizacion, vigencia) {

            var variable = $("#icoreavfoclocres-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasreavfoclocres-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia);
            var imgmenos = document.getElementById("imgmenosreavfoclocres-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia);
            if (variable === "+") {
                $("#icoreavfoclocres-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icoreavfoclocres-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

        }

        vm.AbrirNivelLocalizacionResumenMetas = function (fuenteid, dimensionId, indexcategoria, objetivoId, indexobjetivo, productoId, indexproducto, LocalizacionId, indexlocalizacion, vigencia) {

            var variable = $("#icoreavfoclocmeta-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasreavfoclocmeta-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia);
            var imgmenos = document.getElementById("imgmenosreavfoclocmeta-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia);
            if (variable === "+") {
                $("#icoreavfoclocmeta-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icoreavfoclocmeta-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

        }

        vm.AbrirNivelLocalizacionResumenBeneficiarios = function (fuenteid, dimensionId, indexcategoria, objetivoId, indexobjetivo, productoId, indexproducto, LocalizacionId, indexlocalizacion, vigencia) {

            var variable = $("#icoreavfoclocben-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasreavfoclocben-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia);
            var imgmenos = document.getElementById("imgmenosreavfoclocben-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia);
            if (variable === "+") {
                $("#icoreavfoclocben-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icoreavfoclocben-" + fuenteid + "-" + dimensionId + "-" + indexcategoria + "-" + objetivoId + "-" + indexobjetivo + "-" + productoId + "-" + indexproducto + "-" + LocalizacionId + "-" + indexlocalizacion + "-" + vigencia).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

        }

        vm.MostrarTabResumen = function (origen, fuenteid, dimensionid, numeroobjetivo, productoid, localizacionid) {

            var localizacion = vm.FocalizacionProgramacionSeguimientoDetalle;
            vm.DetallePolitica.Fuentes.forEach(fuente => {
                if (fuente.FuenteId == fuenteid) {
                    fuente.Categorias.forEach(cat => {
                        if (cat.DimensionId == dimensionid) {
                            cat.Objetivos.forEach(obj => {
                                if (obj.NumeroObjetivo == numeroobjetivo) {
                                    obj.Productos.forEach(pro => {
                                        if (pro.ProductoId == productoid) {
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });

            var recursostable = document.getElementById("resumenfocrecursos" + fuenteid + "-" + dimensionid + "-" + numeroobjetivo + "-" + productoid + "-" + localizacionid);
            var metastable = document.getElementById("resumenfocmetas" + fuenteid + "-" + dimensionid + "-" + numeroobjetivo + "-" + productoid + "-" + localizacionid);
            var beneficiariostable = document.getElementById("resumenfocbeneficiarios" + fuenteid + "-" + dimensionid + "-" + numeroobjetivo + "-" + productoid + "-" + localizacionid);

            if (localizacion != "") {
                switch (origen) {
                    case 1:
                        {
                            if (recursostable != undefined) {
                                recursostable.classList.remove('hidden');
                            }
                            if (metastable != undefined) {
                                metastable.classList.add('hidden');
                            }
                            if (beneficiariostable != undefined) {
                                beneficiariostable.classList.add('hidden');
                            }

                            var vigentes = 0;
                            var compromisos = 0;
                            var obligaciones = 0;
                            var pagos = 0;

                            localizacion.RecursosPeriodosActivos.forEach(loc => {
                                vigentes += loc.RecursosVigentesMes;
                                compromisos += loc.RecursosCompromisosMes;
                                obligaciones += loc.RecursosObligacionesMes;
                                pagos += loc.RecursosPagosMes;
                            });

                            vm.DetallePolitica.Fuentes.forEach(fuente => {
                                if (fuente.FuenteId == fuenteid) {
                                    fuente.Categorias.forEach(cat => {
                                        if (cat.DimensionId == dimensionid) {
                                            cat.Objetivos.forEach(obj => {
                                                if (obj.NumeroObjetivo == numeroobjetivo) {
                                                    obj.Productos.forEach(pro => {
                                                        if (pro.ProductoId == productoid) {
                                                        }
                                                    });
                                                }
                                            });
                                        }
                                    });
                                }
                            });

                            break;
                        }
                    case 2:
                        {
                            if (metastable != undefined) {
                                metastable.classList.remove('hidden');
                            }
                            if (recursostable != undefined) {
                                recursostable.classList.add('hidden');
                            }
                            if (beneficiariostable != undefined) {
                                beneficiariostable.classList.add('hidden');
                            }

                            var AcumuladoMesAnteriorIndicadorPpal = 0;
                            var MetaAvanceIndicadorPpalMes = 0;
                            var AcumuladoMesAnteriorIndicadorSec = 0;
                            var MetaAvanceIndicadorSecMes = 0;

                            localizacion.MetasPeriodosActivos.forEach(loc => {
                                AcumuladoMesAnteriorIndicadorPpal += loc.AcumuladoMesAnteriorIndicadorPpal;
                                MetaAvanceIndicadorPpalMes += loc.MetaAvanceIndicadorPpalMes;
                                AcumuladoMesAnteriorIndicadorSec += loc.AcumuladoMesAnteriorIndicadorSec;
                                MetaAvanceIndicadorSecMes += loc.MetaAvanceIndicadorSecMes;
                            });

                            vm.DetallePolitica.Fuentes.forEach(fuente => {
                                if (fuente.FuenteId == fuenteid) {
                                    fuente.Categorias.forEach(cat => {
                                        if (cat.DimensionId == dimensionid) {
                                            cat.Objetivos.forEach(obj => {
                                                if (obj.NumeroObjetivo == numeroobjetivo) {
                                                    obj.Productos.forEach(pro => {
                                                        if (pro.ProductoId == productoid) {
                                                        }
                                                    });
                                                }
                                            });
                                        }
                                    });
                                }
                            });


                            break;
                        }
                    case 3:
                        {
                            if (beneficiariostable != undefined) {
                                beneficiariostable.classList.remove('hidden');
                            }
                            if (recursostable != undefined) {
                                recursostable.classList.add('hidden');
                            }
                            if (metastable != undefined) {
                                metastable.classList.add('hidden');
                            }

                            var AcumuladoMesAnteriorBeneficiarios = 0;
                            var AvanceBeneficiariosMes = 0;


                            localizacion.BeneficiariosPeriodosActivos.forEach(loc => {
                                AcumuladoMesAnteriorBeneficiarios += loc.AcumuladoMesAnteriorBeneficiarios;
                                AvanceBeneficiariosMes += loc.AvanceBeneficiariosMes;
                            });

                            vm.DetallePolitica.Fuentes.forEach(fuente => {
                                if (fuente.FuenteId == fuenteid) {
                                    fuente.Categorias.forEach(cat => {
                                        if (cat.DimensionId == dimensionid) {
                                            cat.Objetivos.forEach(obj => {
                                                if (obj.NumeroObjetivo == numeroobjetivo) {
                                                    obj.Productos.forEach(pro => {
                                                        if (pro.ProductoId == productoid) {
                                                        }
                                                    });
                                                }
                                            });
                                        }
                                    });
                                }
                            });

                            break;
                        }
                }
            }
        }

        vm.habilitarEditar = function (localizacion, fuenteid, categoriaid, indexcategoria, numeroobjetivo, indexobjetivo, productoid, indexproducto, localizacionid, indexlocalizacion) {
            localizacion.HabilitaEditar = true;
            $("#Guardar" + fuenteid + "-" + categoriaid + "-" + indexcategoria + "-" + numeroobjetivo + "-" + indexobjetivo + "-" + productoid + "-" + indexproducto + "-" + localizacionid + "-" + indexlocalizacion).attr('disabled', false);

            vm.FocalizacionProgramacionSeguimientoDetalleorigen = "";
            vm.FocalizacionProgramacionSeguimientoDetalleorigen = JSON.stringify(vm.FocalizacionProgramacionSeguimientoDetalle);

        }

        vm.cancelarEdicion = function (localizacion, politicaid, fuenteid, categoriaid, indexcategoria, numeroobjetivo, indexobjetivo, productoid, indexproducto, localizacionid, indexlocalizacion) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                $("#Guardar" + fuenteid + "-" + categoriaid + "-" + indexcategoria + "-" + numeroobjetivo + "-" + indexobjetivo + "-" + productoid + "-" + indexproducto + "-" + localizacionid + "-" + indexlocalizacion).attr('disabled', true);
                vm.FocalizacionProgramacionSeguimientoDetalle = "";
                vm.FocalizacionProgramacionSeguimientoDetalle = JSON.parse(vm.FocalizacionProgramacionSeguimientoDetalleorigen);


                ConsultarDetallePolitica(politicaid)

                vm.DetallePolitica.Fuentes.forEach(fuente => {
                    if (fuente.FuenteId == fuenteid) {
                        fuente.Categorias.forEach(cat => {
                            if (cat.DimensionId == categoriaid) {
                                cat.Objetivos.forEach(obj => {
                                    if (obj.NumeroObjetivo == numeroobjetivo) {
                                        obj.Productos.forEach(pro => {
                                            if (pro.ProductoId == productoid) {
                                                localizacion.HabilitaEditar = false;
                                            }
                                        });
                                    }
                                });
                            }
                        });
                    }
                });

                setTimeout(function () {
                    utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.")
                }, 500);


            }, function funcionCancelar(reason) {
            }, 'Aceptar', 'Cancelar', 'Los posibles datos que haya diligenciado se perderán.');

        }

        vm.validateFormat = function (event, cantidad) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 12;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

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

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 15 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 15) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
            }
        }

        vm.validarTamanio = function (event, cantidad) {
            var regexp = /^\d+\.\d{0,2}$/;
            var valida = regexp.test(event.target.value);

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
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > cantidad) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        }

        vm.verNombreCompleto = function (idElement, indexElement) {
            var elValidacion = document.getElementById(idElement + indexElement + '-val');
            var elCorto = document.getElementById(idElement + indexElement + '-min');
            var elCompleto = document.getElementById(idElement + indexElement + '-max');

            if (elCompleto.classList.contains('hidden')) {
                elValidacion.innerHTML = 'VER MENOS';
                elCorto.classList.add('hidden');
                elCompleto.classList.remove('hidden');
            } else {
                elValidacion.innerHTML = 'VER MÁS';
                elCorto.classList.remove('hidden');
                elCompleto.classList.add('hidden');
            }
        }

        vm.GuardarAjustes = function (localizacion, politicaid, fuenteid, dimensionid, indexcategoria, numeroobjetivo, indexobjetivo, productoid, indexproducto, localizacionid, indexlocalizacion) {
            var valor = 0;
            vm.ProductoCategoriaGuardar = {
                ProyectoId: vm.FocalizacionProgramacionSeguimiento.ProyectoId,
                BPIN: vm.FocalizacionProgramacionSeguimiento.BPIN,
                DatosFocaliza: []
            };

            var DatosFocaliza = [

            ];


            var df = {
                PoliticaId: politicaid,
                FuenteId: fuenteid,
                DimensionId: dimensionid,
                ProductoId: productoid,
                LocalizacionId: localizacionid,
                Recursos: [],
                Metas: [],
                Beneficiarios: []
            };


            var Recursos = [];
            vm.FocalizacionProgramacionSeguimientoDetalle.RecursosPeriodosActivos.forEach(rec => {
                var recursos = {
                    PeriodoProyectoId: rec.PeriodoProyectoId,
                    PeriodosPeriodicidadId: rec.PeriodosPeriodicidadId,
                    VigenteDelMes: rec.RecursosVigentesMes,
                    Compromisos: rec.RecursosCompromisosMes.toString().includes(",") ? rec.RecursosCompromisosMes.replace(',', '.') : rec.RecursosCompromisosMes,
                    Obligaciones: rec.RecursosObligacionesMes.toString().includes(",") ? rec.RecursosObligacionesMes.replace(',', '.') : rec.RecursosObligacionesMes,
                    Pagos: rec.RecursosPagosMes.toString().includes(",") ? rec.RecursosPagosMes.replace(',', '.') : rec.RecursosPagosMes,
                    Observacion: rec.ObservacionRecurso
                }
                Recursos.push(recursos);
            });
            df.Recursos = Recursos

            var Metas = [];
            vm.FocalizacionProgramacionSeguimientoDetalle.MetasPeriodosActivos.forEach(met => {
                var metas = {
                    PeriodoProyectoId: met.PeriodoProyectoId,
                    PeriodosPeriodicidadId: met.PeriodosPeriodicidadId,
                    AvanceIndicadorPpalMes: met.MetaAvanceIndicadorPpalMes,
                    AvanceIndicadorSecMes: met.MetaAvanceIndicadorSecMes,
                    Observacion: met.ObservacionMeta
                }
                Metas.push(metas);
            });
            df.Metas = Metas

            var Beneficiarios = [];
            vm.FocalizacionProgramacionSeguimientoDetalle.BeneficiariosPeriodosActivos.forEach(ben => {
                var benef = {
                    PeriodoProyectoId: ben.PeriodoProyectoId,
                    PeriodosPeriodicidadId: ben.PeriodosPeriodicidadId,
                    AvanceBeneficiariosMes: ben.AvanceBeneficiariosMes,
                    Observacion: ben.ObservacionBeneficiarios
                }
                Beneficiarios.push(benef);
            });
            df.Beneficiarios = Beneficiarios

            vm.ProductoCategoriaGuardar.DatosFocaliza.push(df);

            return focalizacionPoliticasServicio.GuardarProductoCategoriaSeguimiento(vm.ProductoCategoriaGuardar, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(function (response) {
                if (response.statusText === "OK" || response.status === 200) {                                       
                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    vm.limpiarErrores();
                    CargarDetalleLocalizacion(fuenteid, productoid, localizacionid, dimensionid);
                    ActualizarFocalizacionProgramacionSeguimiento(fuenteid);
                    $("#Guardar" + fuenteid + "-" + dimensionid + "-" + indexcategoria + "-" + numeroobjetivo + "-" + indexobjetivo + "-" + productoid + "-" + indexproducto + "-" + localizacionid + "-" + indexlocalizacion).attr('disabled', true);
                    localizacion.HabilitaEditar = false;
                    vm.ProductoCategoriaGuardar = "";
                    utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                    //vm.refreshfocalizacion = 'true';
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

        function ActualizarFocalizacionProgramacionSeguimiento(fuenteid) {
            var valor = 0;

            const proyecto = {
                InstanciaId: $sessionStorage.idInstancia,
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Texto: "",
                DetalleLocalizacion: [{
                    FuenteId: null,
                    PoliticaId: null,
                }]
            };
            var parametroConsulta = JSON.stringify(proyecto);

            focalizacionPoliticasServicio.ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        var _result = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                        vm.FocalizacionProgramacionSeguimiento.DetallePoliticas.forEach(detalle => {
                            if (detalle.PoliticaId === vm.DetallePolitica.PoliticaId) {
                                if (detalle.Fuentes != null || detalle.Fuentes != undefined) {
                                    detalle.Fuentes.forEach(fuente => {
                                        if (fuente.FuenteId == fuenteid) {
                                            fuente.ComparativoFocalizacion.forEach(cf => {
                                                _result.DetallePoliticas.forEach(r => {
                                                    if (r.PoliticaId === detalle.PoliticaId) {
                                                        if (r.Fuentes != null || r.Fuentes != undefined) {
                                                            r.Fuentes.forEach(f => {
                                                                if (f.FuenteId === fuente.FuenteId) {
                                                                    f.ComparativoFocalizacion.forEach(rcf => {
                                                                        if (rcf.PeriodosPeriodicidadId === cf.PeriodosPeriodicidadId) {
                                                                            cf.Compromisos = rcf.Compromisos;
                                                                            cf.Obligaciones = rcf.Obligaciones;
                                                                            cf.Pagos = rcf.Pagos;
                                                                        }
                                                                    });
                                                                }
                                                            });
                                                        }
                                                    }
                                                });
                                            });
                                        }
                                    });
                                }
                            }
                        });
                    }
                });
        }

        vm.abrirMensajeQueEsEsto = function () {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Programar Actividades - Desglose de actividades</span>");
        }

        /*--------------------- Agregar - Eliminar niveles / actividades ---------------------*/



        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Programar Actividades", errores);
            var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
            if (errores != undefined) {

                var arr = []
                localStorage.setItem('ErroresRAFOC', JSON.stringify(arr));

                vm.erroresActivos = [];
                localStorage.setItem('ErroresRAFOC', JSON.stringify(vm.erroresActivos));
                JSON.parse(localStorage.getItem('ErroresRAFOC'));
                vm.Errorres = [];
                
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                vm.ejecutarErrores();
                
                localStorage.setItem('ErroresRAFOC', JSON.stringify(vm.erroresActivos));
                vm.PintarErrorGenerales();
                
            }
            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
        }

        vm.PintarErrorGenerales = function () {
            var valor = 0;

            var erroresvalidar = JSON.parse(localStorage.getItem('ErroresRAFOC'));
            if (erroresvalidar.length > 0) {

                erroresvalidar.forEach(error => {

                    if (error.Error == "AVFOG007" || error.Error == "AVFOG006") {
                        var seccionpola = document.getElementById("errorpolitica-" + error.Descripcion.split('|')[0]);
                        if (seccionpola != undefined) {
                            seccionpola.classList.remove('hidden');

                        }

                        var seccionpolcala = document.getElementById("errorfuentemsncompol-" + error.Descripcion.split('|')[0]);
                        if (seccionpolcala != undefined) {
                            seccionpolcala.classList.remove('hidden');

                        }

                    }
                    else {
                        var seccionpol = document.getElementById("errorpolitica-" + error.Descripcion.split('|')[4]);
                        if (seccionpol != undefined) {
                            seccionpol.classList.remove('hidden');

                        }

                        var seccionpolca = document.getElementById("errorfuentemsncompol-" + error.Descripcion.split('|')[4]);
                        if (seccionpolca != undefined) {
                            seccionpolca.classList.remove('hidden');

                        }
                    }

                });
            }
            if (vm.Errorres.length > 0) {

                var lstErrores = "";
                var seccionerrores = document.getElementById("errores-");
                var seccionerroresmsn = document.getElementById("erroresmsn-");
                if (seccionerrores != undefined) {
                    if (seccionerroresmsn != undefined) {
                        
                        seccionerroresmsn.classList.remove('hidden');
                    }
                    seccionerrores.classList.remove('hidden');
                    vm.Errorres.forEach(err => {
                        lstErrores += '<span> ' + err.Error +  '| </span>';
                    });

                    seccionerroresmsn.innerHTML = '<span>' + lstErrores + "</span>";
                }
            }            
        }
        vm.limpiarErrores = function () {


            if (vm.FocalizacionProgramacionSeguimiento != "") {
                vm.FocalizacionProgramacionSeguimiento.DetallePoliticas.forEach(det => {
                    det.Fuentes.forEach(fuen => {

                        var seccioncom = document.getElementById("errorfuentecom-" + fuen.FuenteId + "-" + det.PoliticaId);
                        var validacioncom = document.getElementById("errorfuentemsncom-" + fuen.FuenteId + "-" + det.PoliticaId);

                        if (seccioncom != undefined) {
                            if (validacioncom != undefined) {
                                validacioncom.innerHTML = '<span></span>';
                                seccioncom.classList.add('hidden');
                            }
                        }

                        var seccionobli = document.getElementById("errorfuenteobli-" + fuen.FuenteId + "-" + det.PoliticaId);
                        var validacionobli = document.getElementById("errorfuentemsnobli-" + fuen.FuenteId + "-" + det.PoliticaId);

                        if (seccionobli != undefined) {
                            if (validacionobli != undefined) {
                                validacionobli.innerHTML = '<span></span>';
                                seccionobli.classList.add('hidden');
                            }
                        }

                        var seccionpag = document.getElementById("errorfuentepag-" + fuen.FuenteId + "-" + det.PoliticaId);
                        var validacionpag = document.getElementById("errorfuentemsnpag-" + fuen.FuenteId + "-" + det.PoliticaId);

                        if (seccionpag != undefined) {
                            if (validacionpag != undefined) {
                                validacionpag.innerHTML = '<span></span>';
                                seccionpag.classList.add('hidden');
                            }
                        }

                        fuen.Categorias.forEach(cat => {
                            cat.Objetivos.forEach(obj => {
                                obj.Productos.forEach(pro => {
                                    var seccionpro = document.getElementById("errorproductometa-" + pro.ProductoId);
                                    var validacionpro = document.getElementById("errorproductometamsn-" + pro.ProductoId);

                                    if (seccionpro != undefined) {
                                        if (validacionpro != undefined) {
                                            validacionpro.innerHTML = '<span></span>';
                                            seccionpro.classList.add('hidden');
                                        }
                                    }
                                });
                            });
                        });

                    });
                });

                vm.FocalizacionProgramacionSeguimiento.ListaPoliticas.forEach(pol => {
                    var seccionpol = document.getElementById("errorpolitica-" + pol.PoliticaId);
                    if (seccionpol != undefined) {
                        seccionpol.classList.add('hidden');

                    }


                });

                if (vm.Errorres != undefined && vm.Errorres.length > 0) {
                    vm.Errorres.forEach(er => {
                        var seccionpolca = document.getElementById("errorfuentecompol-" + er.PoliticaId);
                        if (seccionpolca != undefined) {
                            seccionpolca.classList.add('hidden');
                        }
                    })
                }
                vm.Errorres = [];
            }
            var seccionerrores = document.getElementById("errores-");
            var seccionerroresmsn = document.getElementById("erroresmsn-");
            if (seccionerrores != undefined) {
                if (seccionerroresmsn != undefined) {
                    seccionerroresmsn.classList.add('hidden');
                    seccionerrores.classList.add('hidden');
                    seccionerroresmsn.innerHTML = '<span></span>';
                }

            }
        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: p.Descripcion,
                        data: p.Data
                    });
                }
            });

        }

        vm.validarAVFOG001 = function (errores) {
            var valores = errores.descripcion.split('|');

            var seccionavanceben = document.getElementById("errorfuenteavanceben-" + valores[0] + "-" + valores[1] + "-" + valores[2] + "-" + valores[3]);
            var validacionavanceben = document.getElementById("errorfuentemsnavanceben-" + valores[0] + "-" + valores[1] + "-" + valores[2] + "-" + valores[3]);

            if (seccionavanceben != undefined) {
                if (validacionavanceben != undefined) {
                    validacionavanceben.innerHTML = '<span>' + valores[5] + "</span>";
                    seccionavanceben.classList.remove('hidden');
                }
            }

            var erroresgeneral = {
                PoliticaId: valores[6],
                Error: valores[5]
            };

            vm.Errorres.push(erroresgeneral)
        }

        vm.validarAVFOG002 = function (errores) {
            var valores = errores.descripcion.split('|');

            var seccionvig = document.getElementById("errorfuentevig-" + valores[0] + "-" + valores[4]);
            var validacionvig = document.getElementById("errorfuentemsnvig-" + valores[0] + "-" + valores[4]);

            if (seccionvig != undefined) {
                if (validacionvig != undefined) {
                    validacionvig.innerHTML = '<span>' + valores[3] + "</span>";
                    seccionvig.classList.remove('hidden');
                }
            }

            var erroresgeneral = {
                PoliticaId: valores[4],
                Error: valores[3]
            };

            vm.Errorres.push(erroresgeneral)
        }

        vm.validarAVFOG003 = function (errores) {
            var valores = errores.descripcion.split('|');

            var seccioncom = document.getElementById("errorfuentecom-" + valores[0] + "-" + valores[4]);
            var validacioncom = document.getElementById("errorfuentemsncom-" + valores[0] + "-" + valores[4]);

            if (seccioncom != undefined) {
                if (validacioncom != undefined) {
                    validacioncom.innerHTML = '<span>' + valores[3] + "</span>";
                    seccioncom.classList.remove('hidden');
                }
            }

            var erroresgeneral = {
                PoliticaId: valores[4],
                Error: valores[3]
            };

            vm.Errorres.push(erroresgeneral)
        }

        vm.validarAVFOG004 = function (errores) {
            var valores = errores.descripcion.split('|');

            var seccionobli = document.getElementById("errorfuenteobli-" + valores[0] + "-" + valores[4]);
            var validacionobli = document.getElementById("errorfuentemsnobli-" + valores[0] + "-" + valores[4]);

            if (seccionobli != undefined) {
                if (validacionobli != undefined) {
                    validacionobli.innerHTML = '<span>' + valores[3] + "</span>";
                    seccionobli.classList.remove('hidden');
                }
            }

            var erroresgeneral = {
                PoliticaId: valores[4],
                Error: valores[3]
            };

            vm.Errorres.push(erroresgeneral)
        }

        vm.validarAVFOG005 = function (errores) {
            var valores = errores.descripcion.split('|');

            var seccionpag = document.getElementById("errorfuentepag-" + valores[0] + "-" + valores[4]);
            var validacionpag = document.getElementById("errorfuentemsnpag-" + valores[0] + "-" + valores[4]);

            if (seccionpag != undefined) {
                if (validacionpag != undefined) {
                    validacionpag.innerHTML = '<span>' + valores[3] + "</span>";
                    seccionpag.classList.remove('hidden');
                }
            }

            var erroresgeneral = {
                PoliticaId: valores[4],
                Error: valores[3]
            };

            vm.Errorres.push(erroresgeneral)

        }

        vm.validarAVFOG006 = function (errores) {
            var valores = errores.descripcion.split('|');
            if (vm.DetallePolitica != "") {
                vm.DetallePolitica.Fuentes.forEach(fuen => {
                    fuen.Categorias.forEach(cat => {
                        cat.Objetivos.forEach(obj => {
                            obj.Productos.forEach(pro => {
                                if (pro.ProductoId == valores[0]) {
                                    var seccionpro = document.getElementById("errorproductometa-" + valores[0]);
                                    var validacionpro = document.getElementById("errorproductometamsn-" + valores[0]);

                                    if (seccionpro != undefined) {
                                        if (validacionpro != undefined) {
                                            validacionpro.innerHTML = '<span>' + valores[2] + "</span>";
                                            seccionpro.classList.remove('hidden');
                                        }
                                    }
                                }
                            });
                        });
                    });
                });

                var erroresgeneral = {
                    PoliticaId: 0,
                    Error: valores[2]
                };

                vm.Errorres.push(erroresgeneral)
            }
        }

        vm.validarAVFOG007 = function (errores) {
            var valores = errores.descripcion.split('|');

            var seccionpag = document.getElementById("errorfuentecat7-" + valores[1] + "-" + valores[0]);
            var validacionpag = document.getElementById("errorfuentemsncat7-" + valores[1] + "-" + valores[0]);

            if (seccionpag != undefined) {
                if (validacionpag != undefined) {
                    validacionpag.innerHTML = '<span>' + valores[4] + "</span>";
                    seccionpag.classList.remove('hidden');
                }
            }

            var erroresgeneral = {
                PoliticaId: valores[0],
                Error: valores[4]
            };

            vm.Errorres.push(erroresgeneral)
        }

        vm.errores = {
            'AVFOG001': vm.validarAVFOG001,
            'AVFOG002': vm.validarAVFOG002,
            'AVFOG003': vm.validarAVFOG003,
            'AVFOG004': vm.validarAVFOG004,
            'AVFOG005': vm.validarAVFOG005,
            'AVFOG006': vm.validarAVFOG006,
            'AVFOG007': vm.validarAVFOG007,
        }

        //****----------- Manejo de Archivos

        function exportExcel(fuenteid) {

            utilidades.mensajeWarning("Si ocurren inconvenientes de descarga o visualización, es necesario actualizar la aplicación.", function funcionContinuar() {

                vm.modelo = null;

                const proyecto = {
                    InstanciaId: $sessionStorage.idInstancia,
                    ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                    Texto: "CargaMasiva",
                    DetalleLocalizacion: [{
                        FuenteId: fuenteid,
                        PoliticaId: vm.DetallePolitica.PoliticaId,
                    }]
                };
                var parametroConsulta = JSON.stringify(proyecto);

                focalizacionPoliticasServicio.ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta).then(resultado => {
                    if (resultado.data != null) {
                        var arreglolistas = jQuery.parseJSON(resultado.data);
                        vm.modelo = jQuery.parseJSON(arreglolistas);


                        const filename = 'Template_.xlsx';
                        const COL_PARAMS = ['hidden', 'wpx', 'width', 'wch', 'MDW'];
                        const STYLE_PARAMS = ['fill', 'font', 'alignment', 'border'];
                        var styleConf = {
                            'E4': {
                                fill: { fgColor: { rgb: 'FFFF0000' } }
                            }
                        };               

                        var columns = [
                            {
                                name: 'DimensionId', title: 'DimensionId'
                            },
                            {
                                name: 'PoliticaId', title: 'PoliticaId'
                            },
                            {
                                name: 'FuenteId', title: 'FuenteId'
                            },
                            {
                                name: 'ObjetivoEspecificoId', title: 'ObjetivoId'
                            },
                            {
                                name: 'ProductoId', title: 'ProductoId'
                            },
                            {
                                name: 'LocalizacionId', title: 'LocalizacionId'
                            },
                            {
                                name: 'PeriodoProyectoId', title: 'PeriodoProyectoId'
                            },
                            {
                                name: 'PeriodosPeriodicidadId', title: 'PeriodosPeriodicidadId'
                            },
                            {
                                name: 'NombrePolitica', title: 'Politica'
                            },
                            {
                                name: 'NombreFuente', title: 'Fuente'
                            },
                            {
                                name: 'Etapa', title: 'Etapa'
                            },
                            {
                                name: 'NombreCategoria', title: 'Categoria'
                            },
                            {
                                name: 'NombreSubCategoria', title: 'SubCategoria'
                            },
                            {
                                name: 'ObjetivoEspecifico ', title: 'Objetivo'
                            },
                            {
                                name: 'NombreProducto', title: 'Producto'
                            },
                            {
                                name: 'CostoProducto', title: 'Costo Producto Vigencia $'
                            },
                            {
                                name: 'Localizacion', title: 'Localizacion'
                            },
                            {
                                name: 'Vigencia', title: 'Vigencia'
                            },
                            {
                                name: 'Mes', title: 'Periodo'
                            },
                            {
                                name: 'RecursosCompromisosMes', title: 'Compromisos $'
                            },
                            {
                                name: 'RecursosObligacionesMes', title: 'Obligaciones $'
                            },
                            {
                                name: 'RecursosPagosMes', title: 'Pagos $'
                            },
                            {
                                name: 'ObservacionRecurso', title: 'Observación Recurso'
                            },
                            {
                                name: 'MetaAvanceIndicadorPpalMes', title: 'Avance Meta Principal'
                            },
                            {
                                name: 'MetaAvanceIndicadorSecMes', title: 'Avance Meta Secundaria'
                            },                           
                            {
                                name: 'ObservacionMeta', title: 'Observacion Meta'
                            },
                            {
                                name: 'AvanceBeneficiariosMes', title: 'Avance Beneficiarios'
                            },
                            {
                                name: 'ObservacionBeneficiarios', title: 'Observación Beneficiarios'
                            }
                        ];

                        let colNames = columns.map(function (item) {
                            return item.title;
                        })

                        var wb = XLSX.utils.book_new();

                        wb.Props = {
                            Title: "Plantilla Seguimiento Focalización",
                            Subject: "PIIP",
                            Author: "PIIP",
                            CreatedDate: new Date().getDate()
                        };

                        wb.SheetNames.push("Hoja Plantilla");

                        const header = colNames;
                        const data = [];

                        vm.modelo.DetallePoliticas.forEach(politica => {
                            if (politica.Fuentes != null) {
                                politica.Fuentes.forEach(fuente => {
                                    if (fuente.Categorias != null) {
                                        fuente.Categorias.forEach(categoria => {
                                            if (categoria.Objetivos != null) {
                                                categoria.Objetivos.forEach(objetivo => {
                                                    if (objetivo.Productos != null) {
                                                        objetivo.Productos.forEach(producto => {
                                                            if (producto.Localizaciones != null) {
                                                                producto.Localizaciones.forEach(localizacion => {
                                                                    localizacion.RecursosPeriodosActivos.forEach(recursopa => {

                                                                        var localiza = "";
                                                                        if (localizacion.Municipio == null) {
                                                                            localiza = localizacion.Departamento;
                                                                        }
                                                                        else {
                                                                            localiza = localizacion.Departamento + " - " + localizacion.Municipio;
                                                                        }

                                                                        data.push({
                                                                            DimensionId: categoria.DimensionId,
                                                                            PoliticaId: politica.PoliticaId,
                                                                            FuenteId: fuente.FuenteId,
                                                                            ObjetivoEspecificoId: objetivo.ObjetivoEspecificoId,
                                                                            ProductoId: producto.ProductoId,
                                                                            LocalizacionId: localizacion.LocalizacionId,
                                                                            PeriodoProyectoId: recursopa.PeriodoProyectoId,
                                                                            PeriodosPeriodicidadId: recursopa.PeriodosPeriodicidadId,
                                                                            NombrePolitica: politica.NombrePolitica,
                                                                            NombreFuente: fuente.NombreFuente,
                                                                            Etapa: producto.Etapa,                                                                           
                                                                            NombreCategoria: categoria.NombreCategoria,
                                                                            NombreSubCategoria: categoria.NombreSubCategoria,
                                                                            ObjetivoEspecifico: objetivo.ObjetivoEspecifico,
                                                                            NombreProducto: producto.NombreProducto,
                                                                            CostoProducto: producto.CostoProducto,
                                                                            Localizacion: localiza,
                                                                            Vigencia: recursopa.Vigencia,
                                                                            Mes: recursopa.Mes,
                                                                            RecursosCompromisosMes: recursopa.RecursosCompromisosMes,
                                                                            RecursosObligacionesMes: recursopa.RecursosObligacionesMes,
                                                                            RecursosPagosMes: recursopa.RecursosPagosMes,
                                                                            ObservacionRecurso: recursopa.ObservacionRecurso,
                                                                            MetaAvanceIndicadorPpalMes: localizacion.MetasPeriodosActivos.find(item => item.Vigencia === recursopa.Vigencia && item.Mes === recursopa.Mes).MetaAvanceIndicadorPpalMes,
                                                                            MetaAvanceIndicadorSecMes: localizacion.MetasPeriodosActivos.find(item => item.Vigencia === recursopa.Vigencia && item.Mes === recursopa.Mes).MetaAvanceIndicadorSecMes,
                                                                            ObservacionMeta: localizacion.MetasPeriodosActivos.find(item => item.Vigencia === recursopa.Vigencia && item.Mes === recursopa.Mes).ObservacionMeta,
                                                                            AvanceBeneficiariosMes: localizacion.BeneficiariosPeriodosActivos.find(item => item.Vigencia === recursopa.Vigencia && item.Mes === recursopa.Mes).AvanceBeneficiariosMes,
                                                                            ObservacionBeneficiarios: localizacion.BeneficiariosPeriodosActivos.find(item => item.Vigencia === recursopa.Vigencia && item.Mes === recursopa.Mes).ObservacionBeneficiarios
                                                                        });
                                                                    });
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

                        const worksheet = XLSX.utils.json_to_sheet(data, {
                            header: ["DimensionId","PoliticaId", "FuenteId", "ObjetivoEspecificoId", "ProductoId", "LocalizacionId", "PeriodoProyectoId", "PeriodosPeriodicidadId", "NombrePolitica", "NombreFuente", "Etapa", "NombreCategoria",
                                "NombreSubCategoria", "ObjetivoEspecifico", "NombreProducto", "CostoProducto", "Localizacion", "Vigencia", "Mes", "RecursosCompromisosMes", "RecursosObligacionesMes", "RecursosPagosMes", "ObservacionRecurso",
                                "MetaAvanceIndicadorPpalMes","MetaAvanceIndicadorSecMes", "ObservacionMeta", "AvanceBeneficiariosMes", "ObservacionBeneficiarios"]
                        });

                        for (let col of [19]) {
                            formatColumn(worksheet, col, "#,##")
                        }
                        for (let col of [20]) {
                            formatColumn(worksheet, col, "#,##")
                        }
                        for (let col of [21]) {
                            formatColumn(worksheet, col, "#,##")
                        }
                        for (let col of [23]) {
                            formatColumn2(worksheet, col, "#,####")
                        }
                        for (let col of [24]) {
                            formatColumn2(worksheet, col, "#,####")
                        }
                        for (let col of [26]) {
                            formatColumn2(worksheet, col, "#,####")
                        }
                        for (let col of [21]) {
                            formatColumn(worksheet, col, "#,##")
                        }
                        for (let col of [23]) {
                            formatColumn2(worksheet, col, "#,####")
                        }
                        for (let col of [24]) {
                            formatColumn2(worksheet, col, "#,####")
                        }
                        for (let col of [26]) {
                            formatColumn2(worksheet, col, "#,####")
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
                        worksheet['!cols'][7] = { hidden: true };


                        wb.Sheets["Hoja Plantilla"] = worksheet;

                        var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                        saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaSeguimientoFocalizacion.xlsx');
                    }
                });
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function formatColumn(worksheet, col) {
            var fmtnumero2 = "#,##0.00";// "##,##";
            var fmtnumero4 = "#,##0.0000";//"#,####";
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

        function formatColumn2(worksheet, col) {
            var fmtnumero2 = "#,##0.0000";
            var fmtnumero4 = "#,##0.0000";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })
                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0") {
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

        $scope.filefNameChanged = function (input) {

            if (input.id.includes(vm.FuenteId)) {
                if (input.files.length == 1) {
                    vm.nombrearchivo = input.files[0].name;
                    document.getElementById('focalizacionnombrearchivo2' + vm.FuenteId).textContent = input.files[0].name;
                    vm.activarControles('cargaarchivo');
                }
                else {
                    vm.filename = input.files.length + " archivos"
                    vm.activarControles('inicio');
                }
            }

        }

        $scope.ChangeSet = function () {
            if (vm.nombrearchivo == "") {
                vm.activarControles('inicio');
            }
        };

        function adjuntarArchivo(FuenteId) {
            vm.FuenteId = FuenteId;
            document.getElementById('filef' + FuenteId).value = "";
            document.getElementById('filef' + FuenteId).click();
        }

        function limpiarArchivo(FuenteId) {
            $scope.files = [];
            document.getElementById('filef' + FuenteId).value = "";
            vm.activarControles2('inicio', FuenteId);
            vm.nombrearchivo = "";
            document.getElementById('focalizacionnombrearchivo2' + FuenteId).textContent = "";
        }

        function limpiaNumero(valor) {
            return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
        }

        vm.validateFormatNegative = function (event) {

            if ((event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            if (event.key == '.') {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 13;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[1], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1) return;
                    if (spiltArray.length === 2) return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 15 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 15) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
            }
        };

        vm.validarTamanioNegative = function (event) {

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
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;

                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        };

        function ValidarDecimal(valor, decimals) {

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

        function validarArchivof() {
            var resultado = true;
            var enajuste = 0;
            var metaEnAjuste = 0;
            vm.FuenteArchivo = [];
            let file = document.getElementById("filef" + vm.FuenteId).files[0];

            if (document.getElementById("filef" + vm.FuenteId).files.length > 0) {

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

                                    if (item["DimensionId"] == undefined) {
                                        utilidades.mensajeError("La columna DimensionId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["DimensionId"])) {
                                        utilidades.mensajeError("El valor DimensionId " + item["DimensionId"] + " no es númerico!");
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

                                    if (item["PeriodosPeriodicidadId"] == undefined) {
                                        utilidades.mensajeError("La columna PeriodosPeriodicidadId no trae valor!");
                                        return false;
                                    } else if (!ValidaSiEsNumero(item["PeriodosPeriodicidadId"])) {
                                        utilidades.mensajeError("El valor PeriodosPeriodicidadId " + item["PeriodosPeriodicidadId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["CostoProducto"] == undefined) {
                                        utilidades.mensajeError("La columna CostoProducto no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["CostoProducto"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de CostoProducto $. La columna CostoProducto $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de CostoProducto " + item["CostoProducto"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["RecursosCompromisosMes"] == undefined) {
                                        utilidades.mensajeError("La columna RecursosCompromisosMes no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["RecursosCompromisosMes"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de RecursosCompromisosMes $ y RecursosObligacionesMes $, RecursosPagosMes $. La columna RecursosCompromisosMes $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de RecursosCompromisosMes " + item["RecursosCompromisosMes"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["RecursosObligacionesMes"] == undefined) {
                                        utilidades.mensajeError("La columna RecursosObligacionesMes no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["RecursosObligacionesMes"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de RecursosCompromisosMes $ y RecursosObligacionesMes $, RecursosPagosMes $. La columna RecursosObligacionesMes $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de RecursosObligacionesMes " + item["RecursosObligacionesMes"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["RecursosPagosMes"] == undefined) {
                                        utilidades.mensajeError("La columna RecursosPagosMes no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["RecursosPagosMes"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de RecursosCompromisosMes $ y RecursosObligacionesMes $, RecursosPagosMes $. La columna RecursosPagosMes $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de RecursosPagosMes " + item["RecursosPagosMes"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["MetaAvanceIndicadorPpalMes"] == undefined) {
                                        utilidades.mensajeError("La columna 'MetaAvanceIndicadorPpalMes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["MetaAvanceIndicadorPpalMes"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'MetaAvanceIndicadorPpalMes' " + item["MetaAvanceIndicadorPpalMes"] + ". La columna 'MetaAvanceIndicadorPpalMes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        metaEnAjuste = item["MetaAvanceIndicadorPpalMes"];
                                    }

                                    if (item["MetaAvanceIndicadorSecMes"] == undefined) {
                                        utilidades.mensajeError("La columna 'MetaAvanceIndicadorSecMes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["MetaAvanceIndicadorSecMes"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'MetaAvanceIndicadorSecMes' " + item["MetaAvanceIndicadorSecMes"] + ". La columna 'MetaAvanceIndicadorSecMes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        metaEnAjuste = item["MetaAvanceIndicadorSecMes"];
                                    }                                  

                                    if (item["AvanceBeneficiariosMes"] == undefined) {
                                        utilidades.mensajeError("La columna 'AvanceBeneficiariosMes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["AvanceBeneficiariosMes"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'AvanceBeneficiariosMes' " + item["AvanceBeneficiariosMes"] + ". La columna 'AvanceBeneficiariosMes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        metaEnAjuste = item["AvanceBeneficiariosMes"];
                                    }

                                    var valoresarchivo = {
                                        DimensionId: item["DimensionId"],
                                        PoliticaId: item["PoliticaId"],
                                        NombrePolitica: item["NombrePolitica"],
                                        FuenteId: item["FuenteId"],
                                        NombreFuente: item["NombreFuente"],
                                        Etapa: item["Etapa"],
                                        ObjetivoEspecificoId: item["ObjetivoEspecificoId"],
                                        ObjetivoEspecifico: item["ObjetivoEspecifico"],
                                        ProductoId: item["ProductoId"],
                                        NombreProducto: item["NombreProducto"],
                                        CostoProductoVigencia: item["CostoProductoVigencia"],
                                        LocalizacionId: item["LocalizacionId"],
                                        Localizacion: item["Localizacion"],
                                        Vigencia: item["Vigencia"],
                                        PeriodoProyectoId: item["PeriodoProyectoId"],
                                        PeriodosPeriodicidadId: item["PeriodosPeriodicidadId"],
                                        NombreCategoria: item["NombreCategoria"],
                                        NombreSubCategoria: item["NombreSubCategoria"],
                                        CostoProducto: item["CostoProducto"],
                                        Mes: item["Mes"],
                                        RecursosCompromisosMes: item["RecursosCompromisosMes"],
                                        RecursosObligacionesMes: item["RecursosObligacionesMes"],
                                        RecursosPagosMes: item["RecursosPagosMes"],
                                        ObservacionRecurso: item["ObservacionRecurso"],
                                        MetaAvanceIndicadorPpalMes: item["MetaAvanceIndicadorPpalMes"],
                                        MetaAvanceIndicadorSecMes: item["MetaAvanceIndicadorSecMes"],
                                        ObservacionMeta: item["ObservacionMeta"],
                                        AvanceBeneficiariosMes: item["AvanceBeneficiariosMes"],
                                        ObservacionBeneficiarios: item["ObservacionBeneficiarios"]
                                    };

                                    vm.FuenteArchivo.push(valoresarchivo);

                                });

                                if (resultado.indexOf(false) == -1) {
                                    vm.activarControles('validado');
                                    utilidades.mensajeSuccess("Proceda a cargar el archivo para que quede registrado en el sistema", false, false, false, "Validación de carga exitosa.");
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
            var aObjetivos = [];
            var aProductos = [];
            var aLocalizaciones = [];
            var aPeriodoProyecto = [];
            var aVigencias = [];

            var existeFuente = 0;
            var existeProducto = 0;
            var existeLocaliacion = 0;
            var existeVigencia = 0;
            var existePeriodoProyecto = 0;
            var CantidadRegistros = 0;

            vm.modelo.DetallePoliticas.forEach(politica => {
                if (politica.Fuentes != null) {
                    politica.Fuentes.forEach(fuente => {
                        if (fuente.Categorias != null) {
                            fuente.Categorias.forEach(categoria => {
                                if (categoria.Objetivos != null) {
                                    categoria.Objetivos.forEach(objetivo => {
                                        CantidadRegistros = CantidadRegistros + 1;
                                    });
                                }
                            });
                        }
                    });
                }
            });
        }

        function GuardarArchivo(valorescero, cargamasiva) {
            vm.activarControles('inicio');

            var registrosArchivo = vm.FuenteArchivo;
            var valor = 0;
            var _DatosFocaliza = [];

            registrosArchivo.forEach(ra => {

                var df = {
                    PoliticaId: ra.PoliticaId,
                    FuenteId: ra.FuenteId,
                    DimensionId: ra.DimensionId,
                    ProductoId: ra.ProductoId,
                    LocalizacionId: ra.LocalizacionId,
                    Recursos: [],
                    Metas: [],
                    Beneficiarios: []
                };


                var Recursos = [];

                var recursos = {
                    PeriodoProyectoId: ra.PeriodoProyectoId,
                    PeriodosPeriodicidadId: ra.PeriodosPeriodicidadId,
                    VigenteDelMes: 0,
                    Compromisos: ra.RecursosCompromisosMes.toString().includes(",") ? ra.RecursosCompromisosMes.replace(',', '.') : ra.RecursosCompromisosMes,
                    Obligaciones: ra.RecursosObligacionesMes.toString().includes(",") ? ra.RecursosObligacionesMes.replace(',', '.') : ra.RecursosObligacionesMes,
                    Pagos: ra.RecursosPagosMes.toString().includes(",") ? ra.RecursosPagosMes.replace(',', '.') : ra.RecursosPagosMes,
                    Observacion: ra.ObservacionRecurso
                }

                Recursos.push(recursos);

                df.Recursos = Recursos

                var Metas = [];
                var metas = {
                    PeriodoProyectoId: ra.PeriodoProyectoId,
                    PeriodosPeriodicidadId: ra.PeriodosPeriodicidadId,
                    AvanceIndicadorPpalMes: ra.MetaAvanceIndicadorPpalMes,
                    AvanceIndicadorSecMes: ra.MetaAvanceIndicadorSecMes,
                    Observacion: ra.ObservacionMeta
                }

                Metas.push(metas);

                df.Metas = Metas

                var Beneficiarios = [];
                var benef = {
                    PeriodoProyectoId: ra.PeriodoProyectoId,
                    PeriodosPeriodicidadId: ra.PeriodosPeriodicidadId,
                    AvanceBeneficiariosMes: ra.AvanceBeneficiariosMes,
                    Observacion: ra.ObservacionBeneficiarios
                }

                Beneficiarios.push(benef);

                df.Beneficiarios = Beneficiarios

                _DatosFocaliza.push(df);
            });

            vm.ProductoCategoriaGuardar = {
                ProyectoId: vm.FocalizacionProgramacionSeguimiento.ProyectoId,
                BPIN: vm.FocalizacionProgramacionSeguimiento.BPIN,
                DatosFocaliza: []
            };

            vm.ProductoCategoriaGuardar.DatosFocaliza = _DatosFocaliza;
            var _dato = _DatosFocaliza[0];

            return focalizacionPoliticasServicio.GuardarProductoCategoriaSeguimiento(vm.ProductoCategoriaGuardar, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(function (response) {
                if (response.statusText === "OK" || response.status === 200) {
                    vm.refreshfocalizacion = 'true';
                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    vm.limpiarErrores();
                    ConsultarDetallePolitica(vm.DetallePolitica.PoliticaId);
                    CargarDetalleLocalizacion(_dato.FuenteId, _dato.ProductoId, _dato.LocalizacionId, _dato.DimensionId);
                    ActualizarFocalizacionProgramacionSeguimiento(vm.DetallePolitica.Fuentes[0].FuenteId);
                    utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");

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

        vm.activarControles = function (evento) {
            switch (evento) {
                case "inicio":
                    $("#btnFocalizacionValidarArchivo" + vm.FuenteId).attr('disabled', true);
                    $("#btnFocalizacionLimpiarArchivo" + vm.FuenteId).attr('disabled', true);
                    $("#btnFocalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', true);
                    document.getElementById('filef' + vm.FuenteId).value = "";
                    document.getElementById('focalizacionnombrearchivo2' + vm.FuenteId).textContent = "";
                    
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnFocalizacionValidarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnFocalizacionValidarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', false);
                    break;
                default:
            }
        }

        vm.activarControles2 = function (evento, FuenteId) {
            switch (evento) {
                case "inicio":
                    $("#btnFocalizacionValidarArchivo" + FuenteId).attr('disabled', true);
                    $("#btnFocalizacionLimpiarArchivo" + FuenteId).attr('disabled', true);
                    $("#btnFocalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', true);
                    document.getElementById('filef' + FuenteId).value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnFocalizacionValidarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado" + FuenteId).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnFocalizacionValidarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado" + FuenteId).attr('disabled', false);
                    break;
                default:
            }
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

        function abrirMensajeArchivoRegionalizacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Plantilla Carga Masiva Focalizacion, </span><br /> <span class='tituhori'><ul><li>No se permite texto en la columna de 'En ajuste $' y 'Meta en ajuste $'</li><li>La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li><li>La columna 'Meta en ajuste' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li></ul></span>");
        }

        //****----------- Fin Manejo de Archivos

        // TODO: Validar que se esté usando
        vm.notificarRefrescoFuentes = null;
        // TODO: Validar que se esté usando
        vm.notificarRefresco = function (handler, nombreComponente) {
            if (nombreComponente == "segfocalizacionsegcrucepoliticas") {
                vm.notificarRefrescoFuentes = handler;
            }
        };

    }

    angular.module('backbone').component('focalizacionPoliticas', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segFocalizacionPoliticas/focalizacionPoliticas/focalizacionPoliticas.html",
        controller: focalizacionPoliticasController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            refreshfocalizacion: '=',
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