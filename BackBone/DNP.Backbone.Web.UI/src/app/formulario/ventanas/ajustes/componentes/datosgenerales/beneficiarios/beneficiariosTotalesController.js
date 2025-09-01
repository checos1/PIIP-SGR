(function () {
    'use strict';

    beneficiariosTotalesController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'beneficiariosServicio',
        'utilidades',
        'justificacionCambiosServicio',
    ];



    function beneficiariosTotalesController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        beneficiariosServicio,
        utilidades,
        justificacionCambiosServicio,
    ) {

        $sessionStorage.vigenciabeneficiariosTotales = '';
        var vm = this;
        vm.init = init;
        vm.lang = "es";
        vm.nombreComponente = "datosgeneralesbeneficiariosTotales";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idProyecto = $sessionStorage.proyectoId;
        vm.codigoBpin = $sessionStorage.idObjetoNegocio;
        vm.habilitaGuardar = false;
        vm.habilitaGuardarProducto = false;
        vm.habilitaAlertaError = false;
        vm.AlertaMensajeError = "";
        vm.ClasesbtnGuardar = "btnguardarDisabledDNP";
        vm.ClasesbtnGuardarProducto = "btnguardarDisabledDNP";
        vm.ClasesinputIni = " ";
        vm.ClasesinputFin = " ";
        vm.mensaje1 = "";
        vm.mensaje2 = "";
        vm.seccionCapitulo = 0;
        vm.habilitarEditar = habilitarEditar;
        vm.habilitarEditarProducto = habilitarEditarProducto;
        vm.habilitarEditarLocalizacion = habilitarEditarLocalizacion;
        vm.habilitarEditarDetalleVigencia = habilitarEditarDetalleVigencia;
        vm.IniciarProducto = IniciarProducto;
        vm.IniciarDetalleBeneficiariosProductoActual = IniciarDetalleBeneficiariosProductoActual;
        vm.habilitar = false;
        vm.habilitarFinal = false;
        vm.habilitarFinalProducto = false;
        vm.verBotones = false;
        vm.evaluarVerBotones = evaluarVerBotones;
        vm.Guardar = Guardar;
        vm.GuardarProducto = GuardarProducto;
        vm.GuardarLocalizacion = GuardarLocalizacion;
        vm.GuardarDetalleVigencia = GuardarDetalleVigencia;
        vm.Usuario = usuarioDNP;
        vm.mostrarOcultar = mostrarOcultar;
        vm.ConvertirNumero = ConvertirNumero;
        vm.beneficiariosTotales = null;
        vm.beneficiariosTotalesDetalle = null;
        vm.beneficiariosTotalesAuxiliar = null;
        vm.productoAuxiliar = null;
        vm.ErrorTotales = false;
        vm.ErrorBeneficiariosTotales = "";
        vm.longMaxText = 30;

        vm.ProductosValidos = true;
        vm.ValidarTotales = ValidarTotales;
        vm.EsValidacion = false;
        vm.TieneErrorCapitulo = false;
        vm.currentYear = new Date().getFullYear();
        vm.alertaBeneficiariosLoc = false;
        vm.componentesRefresh = [
            'recursoscostosdelasacti',
            'datosgeneraleslocalizaciones',
            'datosgeneralesindicadoresdepr',
            'datosgeneraleshorizonte'
        ];

        vm.parametros = {

            idInstancia: $sessionStorage.idInstancia,
            idFlujo: $sessionStorage.idFlujoIframe,
            idNivel: $sessionStorage.idNivel,
            idProyecto: vm.idProyecto,
            idProyectoStr: $sessionStorage.idObjetoNegocio,
            Bpin: vm.Bpin

        };

        vm.mostrarBotones = function (tab, producto) {
            producto.mostrarBotones = tab == 1;
        }

        function ConvertirNumero(numero) {

            if (numero) {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 0,
                }).format(numero);
            }
            else {
                return 0;
            }
        }

        vm.IniciarDetalleVigencias = function (vigencia, producto) {
            vigencia.HabilitaEditar = false;
            vigencia.mostrarCaracteristisca = false;
            if (!vm.EsValidacion) {
                vigencia.ErrorLocalizacion = "";
                vigencia.tieneError = false;
                producto.tieneErrorLocalizacion = false;
                vm.alertaBeneficiariosLoc = false;
            }
            vigencia.numeroPersonasTotalPorVigencia = 0;

            vigencia.textoEditar = "EDITAR";
            if (!vigencia.ClasesbtnGuardarLocalizacion) {
                vigencia.ClasesbtnGuardarLocalizacion = "btnguardarDisabledDNP";
            }
            //producto.LocalizacionProducto.forEach(itemProducto => {
            vm.beneficiariosTotalesDetalle.DetalleLocalizacionBebeficiarios.forEach(itemVigencia => {
                    //itemVigencia.ProductoId = producto.ProductoId;
                    if (itemVigencia.Vigencia == vigencia.Vigencia) {
                        vigencia.numeroPersonasTotalPorVigencia += parseInt(itemVigencia.ValorActual);
                    }

                    if (!itemVigencia.ValorActual) {
                        itemVigencia.ValorActual = 0;
                    }
                });
            //});

            vigencia.numeroPersonasCaracteristica = 0;

            vigencia.totalClasificacion = 0;

            if (vigencia != undefined && vigencia.DetalleCaracteristicaPoblacional != undefined) {
                vigencia.DetalleCaracteristicaPoblacional.forEach(itemCaracteristica => {

                    if (itemCaracteristica != undefined) {
                        itemCaracteristica.totalClasificacion = 0;
                        if (itemCaracteristica.Clasificacion.toUpperCase() != 'Población Vulnerable'.toUpperCase()) {
                            vigencia.DetalleCaracteristicaPoblacional.forEach(itemCaracteristicaAuxiliar => {
                                if (itemCaracteristica.Clasificacion.toUpperCase() == itemCaracteristicaAuxiliar.Clasificacion.toUpperCase()) {
                                    itemCaracteristica.totalClasificacion += parseInt(itemCaracteristicaAuxiliar.DetallePersonasAjuste);
                                }
                                else {
                                    itemCaracteristica.totalClasificacion = parseInt(itemCaracteristicaAuxiliar.DetallePersonasAjuste);
                                }
                            });
                        }

                        if (itemCaracteristica.totalClasificacion > vigencia.totalClasificacion) {
                            vigencia.totalClasificacion = itemCaracteristica.totalClasificacion;
                        }
                    }
                });
            }
        }

        vm.verCaracteristica = function (vigencia) {
            vigencia.mostrarCaracteristisca = true;
        }

        vm.ocultarCaracteristica = function (vigencia) {
            vigencia.mostrarCaracteristisca = false;
        }
        function init() {

            vm.ObtenerbeneficiariosTotales($sessionStorage.idInstancia);
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        function Guardar() {
            vm.ErrorTotales = false;
            vm.ErrorBeneficiariosTotales = "";

            ValidarTotales();

            if (!vm.ErrorTotales) {
                beneficiariosServicio.GuardarBeneficiarioTotales(vm.beneficiariosTotales, usuarioDNP)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                            vm.habilitarFinal = !vm.habilitarFinal;
                            vm.habilitaGuardar = !vm.habilitaGuardar;
                            vm.ClasesbtnGuardar = vm.habilitaGuardar ? "btnguardarDNP" : "btnguardarDisabledDNP";

                            vm.ObtenerbeneficiariosTotales(vm.BPIN);
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            return;
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación", false);
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
            else {
                utilidades.mensajeError("Verifique los campos señalados", null, "Hay datos que presentan inconsistencias.");
            }
        }

        function ValidarTotales() {
            if (vm.beneficiariosTotales.NumeroPersonalAjuste == 0) {
                vm.ErrorTotales = true;
                vm.ErrorBeneficiariosTotales = "Este campo debe tener un valor mayor a cero";
            }
            else if (vm.beneficiariosTotales.NumeroPersonalAjuste < vm.beneficiariosTotales.TotalPoblacionProductos) {
                vm.ErrorTotales = true;
                vm.ErrorBeneficiariosTotales = "El número total de personas beneficiarias del proyecto debe ser mayor o igual que las personas beneficiarias de cada producto";
            }
        }

        function GuardarProducto(DetalleBeneficiariosProductoActual, producto, localicaciones) {

            ValidarProductos(DetalleBeneficiariosProductoActual);

            if (!DetalleBeneficiariosProductoActual.ErrorTotales) {

                DetalleBeneficiariosProductoActual.ProductoId = producto.ProductoId;
                DetalleBeneficiariosProductoActual.ProyectoId = vm.beneficiariosTotales.ProyectoId;

                DetalleBeneficiariosProductoActual.ListaDetalleLocalizacion = [];

                localicaciones.forEach(p => {
                    if (p.SeleccionadoActual) {
                        var detalleLocalizacion = { Id: p.Id };

                        DetalleBeneficiariosProductoActual.ListaDetalleLocalizacion.push(detalleLocalizacion);
                    }
                });

                console.log('ListaDetalleLocalizacion: ', DetalleBeneficiariosProductoActual.ListaDetalleLocalizacion);

                beneficiariosServicio.GuardarBeneficiarioProducto(DetalleBeneficiariosProductoActual, usuarioDNP)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            guardarCapituloModificado();
                            vm.habilitarFinal = !vm.habilitarFinal;
                            vm.habilitaGuardar = !vm.habilitaGuardar;
                            vm.ClasesbtnGuardar = vm.habilitaGuardar ? "btnguardarDNP" : "btnguardarDisabledDNP";

                            vm.ObtenerbeneficiariosTotales(vm.BPIN);
                            
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            return;
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación", false);
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
            else {
                utilidades.mensajeError("Verifique los campos señalados", null, "Hay datos que presentan inconsistencias.");
            }
        }

        function guardarCapituloModificado() {
            const span = document.getElementById('id-capitulo-datosgeneralesbeneficiarios');
            vm.seccionCapitulo = span.textContent;
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: true,
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    console.log(response);
                    if (response.data.Exito) {
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function ValidarProductos(DetalleBeneficiariosProductoActual) {

            DetalleBeneficiariosProductoActual.ErrorTotales = false;
            DetalleBeneficiariosProductoActual.ErrorBeneficiariosTotales = "";

            if (DetalleBeneficiariosProductoActual.PersonasBeneficiaros <= 0) {
                DetalleBeneficiariosProductoActual.ErrorTotales = true;
                DetalleBeneficiariosProductoActual.ErrorBeneficiariosTotales = "Este campo debe tener un valor mayor a cero";

                vm.ProductosValidos = false;
            }
            else if (DetalleBeneficiariosProductoActual.PersonasBeneficiaros > vm.beneficiariosTotales.NumeroPersonalAjuste) {
                DetalleBeneficiariosProductoActual.ErrorTotales = true;
                DetalleBeneficiariosProductoActual.ErrorBeneficiariosTotales = "Las personas beneficiarias del producto no deben superar el número total de personas beneficiarias del proyecto";

                vm.ProductosValidos = false;
            }
        }

        function GuardarLocalizacion(localizacion, producto) {

            let tieneError = false;



            console.log('Localización: ', localizacion);

            tieneError = ValidarLocalizacion(producto, tieneError, localizacion);

            if (!tieneError) {

                var dto = {
                    ProductoId: producto.ProductoId,
                    ProyectoId: vm.idProyecto,
                    LocalizacionId: localizacion.LocalizacionId,
                    DetalleVigencias: vm.beneficiariosTotalesDetalle.DetalleLocalizacionBebeficiarios
                }

                beneficiariosServicio.GuardarBeneficiarioProductoLocalizacion(dto, usuarioDNP)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            guardarCapituloModificado();
                            ToggleEditarCancelarLocalizacion(localizacion);

                            vm.ObtenerbeneficiariosTotales(vm.BPIN);
                            
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });

                            return;
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación", false);
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
            else {
                utilidades.mensajeError("Verifique los campos señalados", null, "Hay datos que presentan inconsistencias.");
            }
        }

        function ValidarLocalizacion(producto, tieneError, localizacion) {

            producto.numeroPersonasTotalProducto = 0;

            //producto.LocalizacionProducto.forEach(itemProducto => {
            vm.beneficiariosTotalesDetalle.DetalleLocalizacionBebeficiarios.forEach(itemVigencia => {

                    if (!vm.EsValidacion) {
                        itemVigencia.ErrorLocalizacion = "";
                        itemVigencia.tieneError = false;
                        localizacion.tieneError = false;
                        producto.tieneErrorLocalizacion = false;
                        vm.alertaBeneficiariosLoc = true;
                        itemVigencia.claseError = "";
                    }

                    producto.numeroPersonasTotalProducto += parseInt(itemVigencia.ValorActual);

                    itemVigencia.numeroPersonasTotalPorVigencia = 0;

                vm.beneficiariosTotalesDetalle.DetalleLocalizacionBebeficiarios.forEach(itemVigenciaAux => {

                        if (itemVigencia.Vigencia == itemVigenciaAux.Vigencia) {
                            itemVigencia.numeroPersonasTotalPorVigencia += parseInt(itemVigenciaAux.ValorActual);
                        }
                    });

                });
            //});

            //localizacion.DetalleVigencias.forEach(itemVigencia => {
                //itemVigencia.LocalizacionId = localizacion.LocalizacionId;

                //itemVigencia.numeroPersonasTotalPorVigencia = 0;

                //producto.LocalizacionProducto.forEach(itemLocalizacion => {
                    //itemLocalizacion.DetalleVigencias.forEach(itemVigenciaAux => {
                        //if (itemVigencia.Vigencia == itemVigenciaAux.Vigencia) {
                            //itemVigencia.numeroPersonasTotalPorVigencia += parseInt(itemVigenciaAux.ValorActual);
                        //}
                    //});
                //});
            //});


            if (producto.DetalleBeneficiariosProductoActual[0].EsAcumulable == 1) {
                if (producto.numeroPersonasTotalProducto > producto.DetalleBeneficiariosProductoActual[0].PersonasBeneficiaros) {
                    producto.ErrorLocalizacion = "El número de personas, no debe superar la población beneficiaria del producto";
                    producto.tieneErrorLocalizacion = true;
                    tieneError = true;
                }
            }

            if (!tieneError) {
                vm.beneficiariosTotalesDetalle.DetalleLocalizacionBebeficiarios.forEach(itemVigencia => {

                    if (!vm.EsValidacion) {
                        producto.ErrorLocalizacion = "";
                        itemVigencia.tieneError = false;
                        localizacion.tieneError = false;
                        producto.tieneErrorLocalizacion = false;
                        vm.alertaBeneficiariosLoc = false;
                        itemVigencia.claseError = "";
                        producto.ErrorLocalizacion = "";
                    }

                    if (itemVigencia.numeroPersonasTotalPorVigencia > producto.DetalleBeneficiariosProductoActual[0].PersonasBeneficiaros) {
                        tieneError = true;
                        itemVigencia.tieneError = true;
                        itemVigencia.claseError = "editInputErrorDNP";
                        producto.tieneErrorLocalizacion = true;
                        vm.alertaBeneficiariosLoc = true;
                        producto.ErrorLocalizacion = "El número de personas por vigencia, no debe superar la población beneficiaria del producto";
                    }
                });
            }

            if (!tieneError) {
                vm.beneficiariosTotalesDetalle.DetalleLocalizacionBebeficiarios.forEach(itemVigencia => {

                    if (!vm.EsValidacion) {
                        itemVigencia.ErrorLocalizacion = "";
                        itemVigencia.tieneError = false;
                        localizacion.tieneError = false;
                        producto.tieneErrorLocalizacion = false;
                        vm.alertaBeneficiariosLoc = false;
                        itemVigencia.claseError = "";
                    }

                    if (itemVigencia.ValorActual < itemVigencia.totalClasificacion) {
                        tieneError = true;
                        itemVigencia.tieneError = true;
                        itemVigencia.claseError = "editInputErrorDNP";
                        localizacion.tieneError = true;
                        producto.tieneErrorLocalizacion = true;
                        vm.alertaBeneficiariosLoc = true;
                        localizacion.error = 'El "Número de personas en ajuste" por vigencia, no puede ser inferior al valor ingresado en la "Característica poblacional"';
                    }
                });
            }
            return tieneError;
        }

        function GuardarDetalleVigencia(vigencia, localizacion, producto) {

            let tieneError = ValidarDetalleVigencia(vigencia, localizacion, producto);

            if (!tieneError) {

                console.log("Beneficiarios vigencia -> ", vigencia);

                vigencia.DetalleCaracteristicas = vigencia.DetalleCaracteristicaPoblacional;

                beneficiariosServicio.GuardarBeneficiarioProductoLocalizacionCaracterizacion(vigencia, usuarioDNP)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            guardarCapituloModificado();
                            ToggleEditarCancelarLocalizacion(localizacion);

                            vm.ObtenerbeneficiariosTotales(vm.BPIN);
                            
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });

                            return;
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación", false);
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
            else {
                utilidades.mensajeError("Verifique los campos señalados", null, "Hay datos que presentan inconsistencias.");
            }
        }

        function ValidarDetalleVigencia(vigencia, localizacion, producto) {
            let tieneError = false;

            vigencia.LocalizacionId = localizacion.LocalizacionId;

            if (!vm.EsValidacion) {
                vigencia.ErrorLocalizacion = "";
                vigencia.tieneError = false;
                localizacion.tieneError = false;
                producto.tieneErrorLocalizacion = false;
            }

            vigencia.numeroPersonasCaracteristica = 0;
            vigencia.ProyectoId = vm.beneficiariosTotales.ProyectoId;

            vigencia.DetalleCaracteristicaPoblacional.forEach(itemCaracteristica => {

                itemCaracteristica.tieneError = false;
                itemCaracteristica.claseError = "";
                itemCaracteristica.ValorCaracteristica = itemCaracteristica.DetallePersonasAjuste;

                if (itemCaracteristica.DetallePersonasAjuste && parseInt(itemCaracteristica.DetallePersonasAjuste) != 0 && !itemCaracteristica.FuenteInformacion) {

                    itemCaracteristica.tieneError = true;
                    vigencia.tieneError = true;
                    localizacion.tieneError = true;
                    producto.tieneErrorLocalizacion = true;
                    vm.alertaBeneficiariosLoc = true;
                    vigencia.ErrorLocalizacion = "Campo obligatorio, fuente de la información";
                    tieneError = true;
                    itemCaracteristica.claseError = "editInputErrorDNP";
                }
                else if (itemCaracteristica.FuenteInformacion && (!itemCaracteristica.DetallePersonasAjuste || parseInt(itemCaracteristica.DetallePersonasAjuste) == 0)) {

                    itemCaracteristica.tieneError = true;
                    vigencia.tieneError = true;
                    localizacion.tieneError = true;
                    producto.tieneErrorLocalizacion = true;
                    vm.alertaBeneficiariosLoc = true;
                    vigencia.ErrorLocalizacion = "Campo obligatorio, número de personas en ajuste";
                    tieneError = true;
                    itemCaracteristica.claseErrorAjuste = "editInputErrorDNP";
                }

                itemCaracteristica.tieneError = false;

                if (itemCaracteristica.Clasificacion.toUpperCase() != 'Población Vulnerable'.toUpperCase()) {

                    itemCaracteristica.totalClasificacion = 0;

                    vigencia.DetalleCaracteristicaPoblacional.forEach(itemCaracteristicaAuxiliar => {

                        if (itemCaracteristica.Clasificacion.toUpperCase() == itemCaracteristicaAuxiliar.Clasificacion.toUpperCase()) {
                            itemCaracteristica.totalClasificacion += parseInt(itemCaracteristicaAuxiliar.DetallePersonasAjuste);
                        }
                    });

                    vigencia.numeroPersonasCaracteristica += parseInt(itemCaracteristica.DetallePersonasAjuste);
                }
                else {
                    itemCaracteristica.totalClasificacion = parseInt(itemCaracteristica.DetallePersonasAjuste);
                }
            });

            if (!tieneError) {
                vigencia.DetalleCaracteristicaPoblacional.forEach(itemCaracteristicaAuxiliar => {
                    if (itemCaracteristicaAuxiliar.DetallePersonasAjuste > vigencia.ValorActual) {
                        itemCaracteristicaAuxiliar.tieneError = true;
                        vigencia.tieneError = true;
                        vigencia.ErrorLocalizacion = "El número de personas por cada tipo de clasificación no debe superar el valor total de personas beneficiarias del producto";
                        localizacion.tieneError = true;
                        producto.tieneErrorLocalizacion = true;
                        vm.alertaBeneficiariosLoc = true;
                        tieneError = true;
                        itemCaracteristicaAuxiliar.claseErrorAjuste = "editInputErrorDNP";
                    }
                });
            }
            return tieneError;
        }

        function habilitarEditar() {

            if (vm.habilitaGuardar == true) {
                utilidades.mensajeWarning("Los posibles datos que haya diligenciado en la tabla 'Detalle beneficiarios totales' se perderan. ¿esta seguro de continuar?", function funcionContinuar() {

                    OkCancelar();

                    vm.beneficiariosTotales = JSON.parse(vm.beneficiariosTotalesAuxiliar);

                    ToggleEditarCancelarTotales();

                }, function funcionCancelar(reason) {
                    return;
                });

            }
            else {
                vm.beneficiariosTotalesAuxiliar = JSON.stringify(vm.beneficiariosTotales);

                ToggleEditarCancelarTotales();
            }
        }

        function ToggleEditarCancelarTotales() {
            vm.habilitarFinal = !vm.habilitarFinal;
            vm.habilitaGuardar = !vm.habilitaGuardar;
            vm.ClasesbtnGuardar = vm.habilitaGuardar ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }

        function IniciarDetalleBeneficiariosProductoActual(DetalleBeneficiariosProductoActual) {
            DetalleBeneficiariosProductoActual.ErrorTotales = false;
            DetalleBeneficiariosProductoActual.ErrorBeneficiariosTotales = "";
        }

        function IniciarProducto(producto) {
            if (!producto.textoEditar) {
                producto.textoEditar = "EDITAR";
            }
            else if (producto.textoEditar == "EDITAR") {
                producto.textoEditar = "CANCELAR";
            }
            else {
                producto.textoEditar = "EDITAR";
            }

            if (!producto.habilitarFinalProducto) {
                producto.habilitarFinalProducto = false;
            }

            if (!producto.habilitaGuardarProducto) {
                producto.habilitaGuardarProducto = false;
            }

            if (!producto.ClasesbtnGuardarProducto) {
                producto.ClasesbtnGuardarProducto = "btnguardarDisabledDNP";
            }

            producto.EsAcumulable = producto.DetalleBeneficiariosProductoActual[0].EsAcumulable;
            producto.EsAcumulableTexto = producto.DetalleBeneficiariosProductoActual[0].EsAcumulable ? "Si" : "No";
            producto.mostrarBotones = true;

            producto.numeroPersonasTotalProducto = 0;

            /*producto.LocalizacionProducto.forEach(itemProducto => {
                itemProducto.DetalleVigencias.forEach(itemVigencia => {
                    producto.numeroPersonasTotalProducto += parseInt(itemVigencia.ValorActual);
                });
            });*/
        }

        vm.AbrirNivel3 = function (localizacion, producto) {

            if (localizacion.mas == 'Img/btnMasTb.svg') {
                localizacion.mas = 'Img/btnMenosTb.svg';
                vm.ObtenerbeneficiariosTotalesDetalle(producto, localizacion);
            }
            else {
                localizacion.mas = 'Img/btnMasTb.svg';
            }

            vm.localizacionConsultada = localizacion.LocalizacionId;
        }

        vm.IniciarDetalleLocalizacion = function (localizacion, producto) {

            localizacion.mas = 'Img/btnMasTb.svg';
            localizacion.ProductoId = producto.ProductoId;
            localizacion.ProyectoId = vm.beneficiariosTotales.ProyectoId;
            localizacion.tieneError = false;
            if (!localizacion.textoEditar) {
                localizacion.textoEditar = "EDITAR";
            }
            else if (localizacion.textoEditar == "EDITAR") {
                localizacion.textoEditar = "CANCELAR";
            }
            else {
                localizacion.textoEditar = "EDITAR";
            }

            if (!localizacion.habilitarFinalLocalizacion) {
                localizacion.habilitarFinalLocalizacion = false;
            }

            if (!localizacion.habilitaGuardarLocalizacion) {
                localizacion.habilitaGuardarLocalizacion = false;
            }

            if (!localizacion.ClasesbtnGuardarLocalizacion) {
                localizacion.ClasesbtnGuardarLocalizacion = "btnguardarDisabledDNP";
            }

            localizacion.mostrarBotones = true;
        }

        function habilitarEditarProducto(producto) {

            if (producto.habilitaGuardarProducto == true) {


                utilidades.mensajeWarning("Los posibles datos que haya diligenciado en la tabla 'Detalle del prodcuto' se perderan. ¿esta seguro de continuar?", function funcionContinuar() {

                    OkCancelar();

                    vm.beneficiariosTotales.BeneficiariosProducto = JSON.parse(vm.productoAuxiliar);

                    ToggleEditarCancelarProducto(producto);

                }, function funcionCancelar(reason) {
                    return;
                });
            }
            else {
                vm.productoAuxiliar = JSON.stringify(vm.beneficiariosTotales.BeneficiariosProducto);

                ToggleEditarCancelarProducto(producto);
            }
        }

        function habilitarEditarLocalizacion(localizacion) {

            if (localizacion.habilitaGuardarLocalizacion == true) {


                utilidades.mensajeWarning("Los posibles datos que haya diligenciado en la tabla 'Detalle localización' se perderan. ¿esta seguro de continuar?", function funcionContinuar() {

                    OkCancelar();

                    vm.beneficiariosTotales.BeneficiariosProducto = JSON.parse(vm.productoAuxiliar);

                    ToggleEditarCancelarLocalizacion(localizacion);

                }, function funcionCancelar(reason) {
                    return;
                });
            }
            else {
                vm.productoAuxiliar = JSON.stringify(vm.beneficiariosTotales.BeneficiariosProducto);

                ToggleEditarCancelarLocalizacion(localizacion);
            }
        }

        function habilitarEditarDetalleVigencia(vigencia) {

            if (vigencia.habilitaGuardarLocalizacion == true) {


                utilidades.mensajeWarning("Los posibles datos que haya diligenciado en la tabla 'Característica poblacional' se perderan. ¿esta seguro de continuar?", function funcionContinuar() {

                    OkCancelar();

                    vm.beneficiariosTotales.BeneficiariosProducto = JSON.parse(vm.productoAuxiliar);

                    ToggleEditarCancelarDetalleVigencia(vigencia);

                }, function funcionCancelar(reason) {
                    return;
                });
            }
            else {
                vm.productoAuxiliar = JSON.stringify(vm.beneficiariosTotales.BeneficiariosProducto);

                ToggleEditarCancelarDetalleVigencia(vigencia);
            }
        }

        function ToggleEditarCancelarProducto(producto) {

            if (producto.textoEditar == "EDITAR") {
                producto.textoEditar = "CANCELAR";
            }
            else {
                producto.textoEditar = "EDITAR";
            }

            producto.habilitarFinalProducto = !producto.habilitarFinalProducto;
            producto.habilitaGuardarProducto = !producto.habilitaGuardarProducto;
            producto.ClasesbtnGuardarProducto = producto.habilitaGuardarProducto ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }

        function ToggleEditarCancelarLocalizacion(localizacion) {

            if (localizacion.textoEditar == "EDITAR") {
                localizacion.textoEditar = "CANCELAR";
            }
            else {
                localizacion.textoEditar = "EDITAR";
            }

            localizacion.habilitarFinalLocalizacion = !localizacion.habilitarFinalLocalizacion;
            localizacion.habilitaGuardarLocalizacion = !localizacion.habilitaGuardarLocalizacion;
            localizacion.ClasesbtnGuardarLocalizacion = localizacion.habilitaGuardarLocalizacion ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }

        function ToggleEditarCancelarDetalleVigencia(vigencia) {

            if (vigencia.textoEditar == "EDITAR") {
                vigencia.textoEditar = "CANCELAR";
            }
            else {
                vigencia.textoEditar = "EDITAR";
            }

            vigencia.habilitarFinalLocalizacion = !vigencia.habilitarFinalLocalizacion;
            vigencia.habilitaGuardarLocalizacion = !vigencia.habilitaGuardarLocalizacion;
            vigencia.ClasesbtnGuardarLocalizacion = vigencia.habilitaGuardarLocalizacion ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
            }, 500);
        }

        //function MostarEdicionProductoCancelado() {
        //	utilidades.mensajeSuccess("Se ha cancelado la edición.", false, false, false);
        //	console.log('MostarEdicionProductoCancelado: ', 'Se ha cancelado la edición.' );
        //      }
        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.ObtenerbeneficiariosTotales(vm.BPIN);
            }
        }

        vm.ObtenerbeneficiariosTotales = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;

            return beneficiariosServicio.ObtenerbeneficiariosTotales($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.beneficiariosTotales = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                        console.log('BeneficiariosTotales: ', vm.beneficiariosTotales);
                    }
                });
        };

        vm.ObtenerbeneficiariosTotalesDetalle = function (productoId, localizacionId) {

            var idInstancia = $sessionStorage.idNivel;

            var parametrosjson = {
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                ProyectoId: vm.idProyecto,
                DetalleLocalizacion: [{
                    ProductoId: productoId.ProductoId,
                    LocalizacionId: localizacionId.LocalizacionId
                }]
            }

            return beneficiariosServicio.ObtenerbeneficiariosTotalesDetalle(JSON.stringify(parametrosjson), usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.beneficiariosTotalesDetalle = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                        console.log('BeneficiariosTotalesDetalle: ', vm.beneficiariosTotalesDetalle);
                    }
                });
        };

        function evaluarVerBotones() {
            vm.verBotones = false;
            if (vm.anioInicioOriginal != vm.anioInicio) {
                vm.verBotones = true;
            }
        }

        function mostrarOcultar(objeto) {
            var variable = $("#icoObjetivo" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#icoObjetivo" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#icoObjetivo" + objeto).attr("src", "Img/btnMas.svg");
            }
        }

        vm.mostrarOcultarProducto = function (objeto) {
            var variable = $("#ico" + objeto).attr("src");

            if (variable === "Img/btnMasn2.svg") {
                $("#ico" + objeto).attr("src", "Img/btnMenosn2.svg");
            }
            else {
                $("#ico" + objeto).attr("src", "Img/btnMasn2.svg");
            }
        }

        vm.mostrarOcultarProductoLoc = function (objeto) {
            var variable = $("#icoloc" + objeto).attr("src");

            if (variable === "Img/btnMasn2.svg") {
                $("#icoloc" + objeto).attr("src", "Img/btnMenosn2.svg");
            }
            else {
                $("#icoloc" + objeto).attr("src", "Img/btnMasn2.svg");
            }

            if (vm.productoConsultado != undefined) {
                if (vm.productoConsultado !== objeto) {
                    $("#icoloc" + vm.productoConsultado).attr("src", "Img/btnMasn2.svg");
                }
            }

            vm.productoConsultado = objeto;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57)) {
                event.preventDefault();
            }
        }

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

            return;
        }

        vm.VerMasMenosProducto = function (indexProducto) {

            $("#div-beneficiario-producto-mas-" + indexProducto).toggleClass('hidden');
            $("#div-beneficiario-producto-menos-" + indexProducto).toggleClass('hidden');
        }

        vm.VerMasMenosProductoLoc = function (indexProducto) {

            $("#div-beneficiario-producto-mas-loc" + indexProducto).toggleClass('hidden');
            $("#div-beneficiario-producto-menos-loc" + indexProducto).toggleClass('hidden');
        }

        /* ------------------------ Validaciones ---------------------------------*/
        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - beneficiariosTotales", errores);
            if (errores != undefined) {

                var isValid = true;

                vm.EsValidacion = true;

                ValidarTotales();

                isValid = !vm.ErrorTotales;

                vm.ProductosValidos = true;
                let tieneError = false;
                let localizacionValida = true;

                vm.beneficiariosTotales.BeneficiariosProducto.forEach(producto => {

                    ValidarProductos(producto.DetalleBeneficiariosProductoActual[0]);

                    producto.LocalizacionProducto.forEach(localizacion => {

                        tieneError = ValidarLocalizacion(producto, tieneError, localizacion);

                        if (tieneError) {
                            localizacionValida = false;

                            isValid = false;
                        }

                        localizacion.DetalleVigencias.forEach(vigencia => {
                            tieneError = ValidarDetalleVigencia(vigencia, localizacion, producto);

                            if (tieneError) {
                                localizacionValida = false;

                                isValid = false;
                            }
                        });
                    });
                });

                vm.EsValidacion = false;

                vm.TieneErrorCapitulo = isValid;

                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };
    }

    angular.module('backbone').component('beneficiariosTotalesProyecto', {

        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/datosgenerales/beneficiarios/beneficiariosTotales.html",
        controller: beneficiariosTotalesController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&'
        }
    }).directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {

                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseInt(value);
                });
            }
        };
    });

})();