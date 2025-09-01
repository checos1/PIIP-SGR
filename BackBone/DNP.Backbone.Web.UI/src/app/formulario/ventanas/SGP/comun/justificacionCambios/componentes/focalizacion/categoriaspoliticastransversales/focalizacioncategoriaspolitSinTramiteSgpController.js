(function () {
    'use strict';

    focalizacioncategoriaspolitSinTramiteSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'justificacionCambiosServicio',
        'constantesBackbone',
        'focalizacioncategoriaspolitSinTramiteSgpServicio'
    ];

    function focalizacioncategoriaspolitSinTramiteSgpController($scope,
        $sessionStorage,
        utilidades,
        justificacionCambiosServicio,
        constantesBackbone,
        focalizacioncategoriaspolitSinTramiteSgpServicio
    ) {

        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "focalizacioncategoriaspolit";
        vm.titulo = "Modificación Focalización";
        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.listaResumen;
        vm.isEdicion;
        vm.isEdicionP;
        vm.CapitulosModificados;
        vm.Capitulo = "Categorías políticas transversales";
        vm.Seccion = "FOCALIZACIÓN";
        vm.validacion = "";
        vm.mensajejustificacion = "Justifique la modificación* (Maximo 8.000 caracteres)";
        vm.seccionCapitulo = null;
        vm.AjustesJustificaionRegionalizacion = [];
        vm.AjustesJustificaionRegionalizacionInicial;
        vm.listaResumen = "";
        vm.SeccionPoliticasDT = [];
        vm.SeccionPoliticasDTBK = [];
        vm.DetalleAjustes = [];
        vm.mensaje = "";
        vm.RecursosRegionalizacionAjuste = [];
        vm.ConvertirNumero = ConvertirNumero;
        vm.VerJustificacionOtrasPoliticas = 0;
        vm.soloLectura = false;
        vm.init = function () {

            obtenerDetalleAjustePorPoliitica();
            setTimeout(function () {
                obtenerSeccionPoliticasAjustadas()
            }, 10000
            )
            vm.editar();
            ObtenerSeccionCapitulo();
            vm.justificacion = vm.justificacioncapitulo;
            vm.notificacioncambios({ handler: vm.notificacionJustificacion });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });

        };
      
        function obtenerDetalleAjustePorPoliitica() {
            return focalizacioncategoriaspolitSinTramiteSgpServicio.ObtenerDetalleCategoriasFocalizacionJustificacionSgp(vm.BPIN, vm.idUsuario).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.DetalleAjustes = respuesta.data.split('|');                        
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            var FaseGuid = constantesBackbone.idEtapaNuevaEjecucion;
            var Capitulo = vm.Capitulo;
            var Seccion = vm.Seccion;
            return justificacionCambiosServicio.ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, $sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.seccionCapitulo = respuesta.data;
                    }
                });
        }

        function obtenerSeccionPoliticasAjustadas() {
            return focalizacioncategoriaspolitSinTramiteSgpServicio.ObtenerCategoriasFocalizacionJustificacionSgp(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.AjustesJustificaionRegionalizacion = jQuery.parseJSON(arreglolistas);

                        var origen = 0;
                        var i = 0;
                        var verpolitica = 0;
                        vm.VerJustificacionOtrasPoliticas = 1;
                        vm.AjustesJustificaionRegionalizacion.Politicas.forEach(poli => {
                            if (poli.DetallePoliticas != null) {
                                poli.DetallePoliticas.forEach(polipol => {
                                    origen = 0;
                                    if (polipol.Recursos != null || polipol.Metas != null || polipol.Personas != null) {
                                        polipol.Visible = 1;

                                        if (polipol.Recursos != null) {
                                            origen = 1;
                                        }
                                        else if (polipol.Metas != null) {
                                            origen = 2;
                                        }
                                        else if (polipol.Personas != null) {
                                            origen = 3;
                                        }
                                        verpolitica = 1;
                                        vm.VerJustificacionOtrasPoliticas = 1;
                                        vm.mostrarTab(origen, poli.PoliticaId, polipol.CategoriaId, polipol.Fuenteid, polipol.ProductoId, polipol.LocalizacionId, i);
                                    }
                                    else {
                                        polipol.Visible = 0;
                                    }
                                });
                                poli.Detalle = [];
                            }                            
                            poli.VerPolitica = verpolitica;
                            verpolitica = 0;
                            i++;
                        });
                        vm.AjustesJustificaionRegionalizacion.Politicas.forEach(poli => {
                            vm.DetalleAjustes.forEach(det => {
                                det.split('*');
                                if (det.split('*')[0] == poli.PoliticaId) {
                                    poli.Detalle.push(det.split('*')[1]);                                  
                                };
                            });
                        });
                    }
                });
        }

        vm.volver = function () {
            $(window).scrollTop($('#justificacionpoliticaspol').position().top);
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }                  

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 0,
            }).format(numero);
        }

        vm.AbrilNivel = function () {

            var variable = $("#ico-otras-politicas")[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-otras-politicas");
            var imgmenos = document.getElementById("imgmenos-otras-politicas");
            if (variable === "+") {
                $("#ico-otras-politicas").html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-otras-politicas").html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.AbrilNivel1 = function (politicaId, indexpoliticas, categoriaId, fuenteId, productoId, localizacionId) {

            var variable = $("#ico-" + politicaId + "-" + indexpoliticas)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + politicaId + "-" + indexpoliticas);
            var imgmenos = document.getElementById("imgmenos-" + politicaId + "-" + indexpoliticas);
            if (variable === "+") {
                $("#ico-" + politicaId + "-" + indexpoliticas).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-" + politicaId + "-" + indexpoliticas).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
            //this.mostrarTab(1, politicaId, categoriaId, fuenteId, productoId, localizacionId, indexpoliticas)

            var origen = 0;
            var i = 0;
            var verpolitica = 0;
            vm.AjustesJustificaionRegionalizacion.Politicas.forEach(poli => {
                if (poli.DetallePoliticas != null) {
                    i = 0;
                    poli.DetallePoliticas.forEach(polipol => {
                        origen = 0;
                        if (polipol.Recursos != null || polipol.Metas != null || polipol.Personas != null) {
                            polipol.Visible = 1;

                            if (polipol.Recursos != null) {
                                origen = 1;
                            }
                            else if (polipol.Metas != null) {
                                origen = 2;
                            }
                            else if (polipol.Personas != null) {
                                origen = 3;
                            }
                            verpolitica = 1;
                            vm.VerJustificacionOtrasPoliticas = 1;
                            vm.mostrarTab(origen, poli.PoliticaId, polipol.CategoriaId, polipol.Fuenteid, polipol.ProductoId, polipol.LocalizacionId, i);
                            i++;
                        }
                        else {
                            polipol.Visible = 0;
                        }
                    });
                    poli.Detalle = [];

                }
                poli.VerPolitica = verpolitica;
                verpolitica = 0;
            });
            vm.AjustesJustificaionRegionalizacion.Politicas.forEach(poli => {

                vm.DetalleAjustes.forEach(det => {
                    det.split('*');
                    if (det.split('*')[0] == poli.PoliticaId) {
                        poli.Detalle.push(det.split('*')[1]);
                    };
                });
            });

        }

        vm.mostrarTab = function (origen, politicaId, categoriaId, fuenteId, productoId, localizacionId, indexpoliticas) {

            let listafinal = [];
            vm.listaResumen;
            var politica = "";

            var ValorFirme = 0;
            var ValorAjuste = 0;
            var ValorDiferencia = 0;

            vm.AjustesJustificaionRegionalizacion.Politicas.forEach(poli => {
                if (poli.DetallePoliticas != null) {
                    poli.DetallePoliticas.forEach(polipol => {
                        if (polipol.PoliticaId == politicaId && polipol.CategoriaId == categoriaId && polipol.Fuenteid == fuenteId && polipol.ProductoId == productoId && polipol.LocalizacionId == localizacionId) {
                            politica = polipol;
                        }
                    });
                }
            });

            var recursostable = document.getElementById("recursos" + politicaId + "-" + categoriaId + "-" + fuenteId + "-" + productoId + "-" + localizacionId + "-" + indexpoliticas);
            var metastable = document.getElementById("metas" + politicaId + "-" + categoriaId + "-" + fuenteId + "-" + productoId + "-" + localizacionId + "-" + indexpoliticas);
            var personastable = document.getElementById("personas" + politicaId + "-" + categoriaId + "-" + fuenteId + "-" + productoId + "-" + localizacionId + "-" + indexpoliticas);

            switch (origen) {
                case 1:
                    {

                        if (recursostable != undefined) {
                            recursostable.classList.remove('hidden');
                        }
                        if (metastable != undefined) {
                            metastable.classList.add('hidden');
                        }
                        if (personastable != undefined) {
                            personastable.classList.add('hidden');
                        }

                        if (politica.Recursos != null) {
                            politica.Recursos.forEach(item => {
                                ValorFirme += parseFloat(item.ValorFirme);
                                ValorAjuste += parseFloat(item.ValorActual);
                                ValorDiferencia += parseFloat(item.Diferencia);
                            });

                            vm.AjustesJustificaionRegionalizacion.Politicas.forEach(pol => {
                                if (pol.DetallePoliticas != null) {
                                    pol.DetallePoliticas.forEach(poli => {
                                        if (pol.PoliticaId == politicaId && poli.CategoriaId == categoriaId && poli.Fuenteid == fuenteId && poli.ProductoId == productoId && poli.LocalizacionId == localizacionId) {
                                            poli.Recursos.ValorFirme = ValorFirme;
                                            poli.Recursos.ValorAjuste = ValorAjuste;
                                            poli.Recursos.ValorDiferencia = ValorDiferencia;
                                        }
                                    });
                                }
                            });
                        }

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
                        if (personastable != undefined) {
                            personastable.classList.add('hidden');
                        }

                        if (politica.Metas != null) {
                            politica.Metas.forEach(item => {
                                ValorFirme += parseFloat(item.MetaFirme);
                                ValorAjuste += parseFloat(item.MetaActual);
                                ValorDiferencia += parseFloat(item.Diferencia);
                            });

                            vm.AjustesJustificaionRegionalizacion.Politicas.forEach(pol => {
                                if (pol.DetallePoliticas != null) {
                                    pol.DetallePoliticas.forEach(poli => {
                                        if (poli.PoliticaId == politicaId && poli.CategoriaId == categoriaId && poli.Fuenteid == fuenteId && poli.ProductoId == productoId && poli.LocalizacionId == localizacionId) {
                                            poli.Metas.ValorFirme = ValorFirme;
                                            poli.Metas.ValorAjuste = ValorAjuste;
                                            poli.Metas.ValorDiferencia = ValorDiferencia;
                                        }
                                    });
                                }
                            });
                        }
                        break;
                    }
                case 3:
                    {

                        if (personastable != undefined) {
                            personastable.classList.remove('hidden');
                        }
                        if (recursostable != undefined) {
                            recursostable.classList.add('hidden');
                        }
                        if (metastable != undefined) {
                            metastable.classList.add('hidden');
                        }

                        if (politica.Personas != null) {
                            politica.Personas.forEach(item => {
                                ValorFirme += parseFloat(item.PersonasFirme);
                                ValorAjuste += parseFloat(item.PersonasActual);
                                ValorDiferencia += parseFloat(item.Diferencia);
                            });

                            vm.AjustesJustificaionRegionalizacion.Politicas.forEach(pol => {
                                if (pol.DetallePoliticas != null) {
                                    pol.DetallePoliticas.forEach(poli => {
                                        if (pol.PoliticaId == politicaId && poli.CategoriaId == categoriaId && poli.Fuenteid == fuenteId && poli.ProductoId == productoId && poli.LocalizacionId == localizacionId) {
                                            poli.Personas.ValorFirme = ValorFirme;
                                            poli.Personas.ValorAjuste = ValorAjuste;
                                            poli.Personas.ValorDiferencia = ValorDiferencia;
                                        }
                                    });
                                }
                            });
                        }
                        break;
                    }
            }

            /*  //*/
        }
       
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.editar = function (estado) {
            vm.isEdicion = null;
            vm.isEdicion = estado == 'editar';
            if (vm.isEdicion) {

                var btnguardar = document.getElementById("btn-guardar-edicion-ff");

                document.getElementById("justificacionfocapt").disabled = false;
                document.getElementById("justificacionfocapt").classList.remove('disabled');
                if (btnguardar != undefined) {
                    document.getElementById("btn-guardar-edicion-ff").classList.remove('disabled');
                }
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
            } else {

                if (estado == 'cancelar') {
                    utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                        OkCancelar();
                        var justificacion = document.getElementById("justificacionfocapt");
                        if (justificacion != undefined) {
                            document.getElementById("justificacionfocapt").disabled = true;
                            document.getElementById("justificacionfocapt").classList.add('disabled');
                            /*document.getElementById("btn-guardar-edicion-ff").classList.add('disabled');*/
                            document.getElementById("justificacionfocapt").value = vm.justificacioncapitulo;
                        }
                        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

                    }, function funcionCancelar(reason) {
                        vm.isEdicion = true;
                        return;
                    }, null, null, "Advertencia");
                }
                else {
                    var justificacion = document.getElementById("justificacionfocapt");
                    if (justificacion != undefined) {
                        document.getElementById("justificacionfocapt").disabled = true;
                        document.getElementById("justificacionfocapt").classList.add('disabled');
                        /*document.getElementById("btn-guardar-edicion-ff").classList.add('disabled');*/
                        document.getElementById("justificacionfocapt").value = vm.justificacioncapitulo;
                    }
                    vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
                }


            }
        }

        vm.guardar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                utilidades.mensajeError('Debe ingresar una justificación.');
                return false;
            }
            
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: vm.justificacion,
                SeccionCapituloId: 909,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                AplicaJustificacion: 1,
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess(response.data.Mensaje);
                        document.getElementById("justificacionfocapt").value = vm.justificacion;
                        vm.justificacioncapitulo = vm.justificacion;
                        vm.editar('');
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje);
                    }
                });
        }

        vm.validar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                vm.validacion = "Debe diligenciar la justificación del capítulo dentro de la pestaña Justificación.";
                return false;
            }
            vm.validacion = "";
            return true;
        }

        vm.notificacionValidacion = function (errores) {
            console.log("Validación  - Justificación Categorías políticas transversales");
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresfocalizacionpt = errores.find(p => p.Capitulo == "focalizacioncategoriaspolit");
                if (erroresfocalizacionpt != undefined) {
                    var erroresJson = erroresfocalizacionpt.Errores == "" ? [] : JSON.parse(erroresfocalizacionpt.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson.errores.forEach(p => {
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }

                }
            }
            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        };

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns");
            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion4 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-4");
            var ValidacionFFR1Error4 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-4");
            if (campoObligatorioJustificacion4 != undefined) {
                if (ValidacionFFR1Error4 != undefined) {
                    ValidacionFFR1Error4.innerHTML = '';
                    campoObligatorioJustificacion4.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion7 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-7");
            var ValidacionFFR1Error7 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-7");
            if (campoObligatorioJustificacion7 != undefined) {
                if (ValidacionFFR1Error7 != undefined) {
                    ValidacionFFR1Error7.innerHTML = '';
                    campoObligatorioJustificacion7.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion9 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-9");
            var ValidacionFFR1Error9 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-9");
            if (campoObligatorioJustificacion9 != undefined) {
                if (ValidacionFFR1Error9 != undefined) {
                    ValidacionFFR1Error9.innerHTML = '';
                    campoObligatorioJustificacion9.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion10 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-10");
            var ValidacionFFR1Error10 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-10");
            if (campoObligatorioJustificacion10 != undefined) {
                if (ValidacionFFR1Error10 != undefined) {
                    ValidacionFFR1Error10.innerHTML = '';
                    campoObligatorioJustificacion10.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion11 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-11");
            var ValidacionFFR1Error11 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-11");
            if (campoObligatorioJustificacion11 != undefined) {
                if (ValidacionFFR1Error11 != undefined) {
                    ValidacionFFR1Error11.innerHTML = '';
                    campoObligatorioJustificacion11.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion12 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-12");
            var ValidacionFFR1Error12 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-12");
            if (campoObligatorioJustificacion12 != undefined) {
                if (ValidacionFFR1Error12 != undefined) {
                    ValidacionFFR1Error12.innerHTML = '';
                    campoObligatorioJustificacion12.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion13 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-13");
            var ValidacionFFR1Error13 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-13");
            if (campoObligatorioJustificacion13 != undefined) {
                if (ValidacionFFR1Error13 != undefined) {
                    ValidacionFFR1Error13.innerHTML = '';
                    campoObligatorioJustificacion13.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion14 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-14");
            var ValidacionFFR1Error14 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-14");
            if (campoObligatorioJustificacion14 != undefined) {
                if (ValidacionFFR1Error14 != undefined) {
                    ValidacionFFR1Error14.innerHTML = '';
                    campoObligatorioJustificacion14.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion20 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-20");
            var ValidacionFFR1Error20 = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns-20");
            if (campoObligatorioJustificacion20 != undefined) {
                if (ValidacionFFR1Error20 != undefined) {
                    ValidacionFFR1Error20.innerHTML = '';
                    campoObligatorioJustificacion20.classList.add('hidden');
                }
            }

        }

        vm.validarJustificacion = function (errores) {

            setTimeout(function () {
                var campoObligatorioJustificacion = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error");
                var ValidacionFFR1Error = document.getElementById("focalizacionpolsgpcategoriapoliticassintramitesgp-justificacion-error-mns");

                if (campoObligatorioJustificacion != undefined) {
                    if (ValidacionFFR1Error != undefined) {
                        ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                        campoObligatorioJustificacion.classList.remove('hidden');
                    }
                }
            }, 0
            );
        }
      
        vm.errores = {
            'JUST001': vm.validarJustificacion,           
        }
    }

    angular.module('backbone').component('focalizacionpolsgpcategoriapoliticassintramitesgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/focalizacion/categoriaspoliticastransversales/focalizacioncategoriaspolitSinTramiteSgp.html",
        controller: focalizacioncategoriaspolitSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            justificacioncapitulo: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();