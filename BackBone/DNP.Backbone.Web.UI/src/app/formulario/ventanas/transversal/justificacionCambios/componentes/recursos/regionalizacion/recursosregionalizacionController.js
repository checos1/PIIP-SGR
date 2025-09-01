(function () {
    'use strict';

    recursosregionalizacionController.$inject = [
        '$scope',
        '$sessionStorage',
        'recursosfuentesdefinancServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'relacionPlanificacionServicio',
        'constantesBackbone',
        'recursosregionalizacionServicio'
    ];

    function recursosregionalizacionController($scope,
        $sessionStorage,
        recursosfuentesdefinancServicio,
        utilidades,
        justificacionCambiosServicio,
        relacionPlanificacionServicio,
        constantesBackbone,
        recursosregionalizacionServicio
    ) {

        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "recursosregionalizacion";
        vm.titulo = "Modificación Regionalización";
        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.listaResumen;
        vm.isEdicion;
        vm.CapitulosModificados;
        vm.Capitulo = "Regionalización";
        vm.Seccion = "RECURSOS";
        vm.validacion = "";
        vm.mensajejustificacion = "Justifique la modificación* (Maximo 8.000 caracteres)";
        vm.seccionCapitulo = null;
        vm.AjustesJustificaionRegionalizacion;
        vm.listaResumen;
        vm.ConvertirNumero = ConvertirNumero;
        vm.tab = 0;
        vm.ConvertirNumero4decimales = ConvertirNumero4decimales;


        vm.DetalleAjustes = [];
        vm.mensaje = "";
        vm.RecursosRegionalizacionAjuste = [];

        vm.init = function () {
            obtenerDetalleAjustesJustificaionRegionalizacion();
            vm.editar();
            ObtenerSeccionCapitulo();
            vm.justificacion = vm.justificacioncapitulo;
            vm.notificacioncambios({ handler: vm.notificacionJustificacion });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });

        };

        function ObtenerSeccionCapitulo() {
            var FaseGuid = constantesBackbone.idEtapaNuevaEjecucion;
            var Capitulo = 'Regionalización';
            var Seccion = 'Recursos';

            return justificacionCambiosServicio.ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, $sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.seccionCapitulo = respuesta.data;
                    }
                });

        }

        function obtenerDetalleAjustesJustificaionRegionalizacion() {
            return recursosregionalizacionServicio.obtenerDetalleAjustesJustificaionRegionalizacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.AjustesJustificaionRegionalizacion = jQuery.parseJSON(arreglolistas);

                        var FuenteId = 0;
                        var ProductoId = 0;
                        var existe = 0;

                        if (vm.AjustesJustificaionRegionalizacion.Recursos != null) {
                            vm.AjustesJustificaionRegionalizacion.Recursos.forEach(ajr => {

                                if (ajr.Diferencia != 0) {
                                    existe = 0;
                                    if (vm.RecursosRegionalizacionAjuste.length > 0) {
                                        vm.RecursosRegionalizacionAjuste.forEach(rr => {
                                            if (ajr.FuenteId == rr.FuenteId && ajr.ProductoId == rr.ProductoId) {
                                                existe = 1;
                                            }
                                        });
                                    }

                                    if (existe == 0) {
                                        var fuente = {
                                            FuenteId: ajr.FuenteId,
                                            Fuente: ajr.Texto + " - Producto: " + ajr.NombreProducto,
                                            ProductoId: ajr.ProductoId,
                                            Recursos: [],
                                            Metas: []
                                        }
                                        vm.RecursosRegionalizacionAjuste.push(fuente);
                                    }
                                }
                            });
                        }

                        FuenteId = 0;
                        ProductoId = 0;

                        if (vm.AjustesJustificaionRegionalizacion.Metas != null) {
                            vm.AjustesJustificaionRegionalizacion.Metas.forEach(ajr => {

                                if (ajr.Diferencia != 0) {
                                    existe = 0;
                                    if (vm.RecursosRegionalizacionAjuste.length > 0) {
                                        vm.RecursosRegionalizacionAjuste.forEach(rr => {
                                            if (ajr.FuenteId == rr.FuenteId && ajr.ProductoId == rr.ProductoId) {
                                                existe = 1;
                                            }
                                        });
                                    }

                                    if (existe == 0) {
                                        var fuente = {
                                            FuenteId: ajr.FuenteId,
                                            Fuente: ajr.Texto + " - Producto: " + ajr.NombreProducto,
                                            ProductoId: ajr.ProductoId,
                                            Recursos: [],
                                            Metas: []
                                        }
                                        vm.RecursosRegionalizacionAjuste.push(fuente);
                                    }
                                }
                            });
                        }

                        var valor = 0;
                        vm.RecursosRegionalizacionAjuste.forEach(rra => {
                            if (vm.AjustesJustificaionRegionalizacion.Recursos != null) {
                                vm.AjustesJustificaionRegionalizacion.Recursos.forEach(ajr => {
                                    if (rra.FuenteId == ajr.FuenteId && rra.ProductoId == ajr.ProductoId) {
                                        if (ajr.Diferencia != 0) {
                                            var recu = {
                                                FuenteId: ajr.FuenteId,
                                                Vigencia: ajr.Vigencia,
                                                Departamento: ajr.Departamento,
                                                Municipio: ajr.Municipio,
                                                TipoAgrupacion: ajr.TipoAgrupacion,
                                                Agrupacion: ajr.Agrupacion,
                                                EnFirme: ajr.ValorFirme,
                                                EnAjuste: ajr.ValorAjuste,
                                                Diferencia: ajr.Diferencia,
                                                ProductoId: ajr.productoId
                                            };
                                            rra.Recursos.push(recu);
                                        }
                                    }
                                });
                            }

                            if (vm.AjustesJustificaionRegionalizacion.Metas != null) {
                                vm.AjustesJustificaionRegionalizacion.Metas.forEach(ajr => {
                                    if (rra.FuenteId == ajr.FuenteId && rra.ProductoId == ajr.ProductoId) {
                                        if (ajr.Diferencia != 0) {
                                            var metas = {
                                                FuenteId: ajr.FuenteId,
                                                Vigencia: ajr.Vigencia,
                                                Departamento: ajr.Departamento,
                                                Municipio: ajr.Municipio,
                                                TipoAgrupacion: ajr.TipoAgrupacion,
                                                Agrupacion: ajr.Agrupacion,
                                                EnFirme: ajr.MetaFirme,
                                                EnAjuste: ajr.MetaAjuste,
                                                Diferencia: ajr.Diferencia,
                                                ProductoId: ajr.productoId
                                            };
                                            rra.Metas.push(metas);
                                        }
                                    }
                                });
                            }

                        });

                        vm.RecursosRegionalizacionAjuste.forEach(rra => {
                            if (rra.Recursos.length > 0)
                                vm.mostrarTab(1, rra.FuenteId, rra.ProductoId);
                            if (rra.Metas.length > 0)
                                vm.mostrarTab(2, rra.FuenteId, rra.ProductoId);
                        });

                    }
                });
        }
        vm.mostrarTab = function (origen, fuenteid, productoId) {
            let listafinal = [];
            vm.listaResumen = [];
            vm.tab = origen;

            switch (origen) {
                case 1:
                    {
                        var tabr = document.getElementById("tabRecursos" + "-" + fuenteid + "-" + productoId);
                        if (tabr != undefined) {
                            //tabr.classList.remove('invisible');
                            tabr.classList.remove('hidden');
                        }

                        var tabm = document.getElementById("tabMetas" + "-" + fuenteid + "-" + productoId);
                        if (tabm != undefined) {
                            tabm.classList.add('hidden');
                            //tabm.classList.add('invisible');
                        }

                        vm.RecursosRegionalizacionAjuste.forEach(rra => {
                            if (rra.FuenteId == fuenteid && rra.ProductoId == productoId) {
                                rra.Recursos.forEach(rec => {
                                    if (rec.Diferencia != 0) {
                                        var resumen = {
                                            Vigencia: rec.Vigencia,
                                            Departamento: rec.Departamento,
                                            Municipio: rec.Municipio,
                                            TipoAgrupacion: rec.TipoAgrupacion,
                                            Agrupacion: rec.Agrupacion,
                                            EnFirme: rec.EnFirme,
                                            EnAjuste: rec.EnAjuste,
                                            Diferencia: rec.Diferencia,
                                        };
                                        listafinal.push(resumen);
                                    }
                                });
                            }
                        });
                        break;
                    }
                case 2:
                    {
                        var tabr = document.getElementById("tabRecursos" + "-" + fuenteid + "-" + productoId);
                        if (tabr != undefined) {
                            //tabr.classList.add('invisible');
                            tabr.classList.add('hidden');
                        }

                        var tabm = document.getElementById("tabMetas" + "-" + fuenteid + "-" + productoId);
                        if (tabm != undefined) {
                            tabm.classList.remove('hidden');
                            //tabm.classList.remove('visible');
                        }

                        vm.RecursosRegionalizacionAjuste.forEach(rra => {
                            if (rra.FuenteId == fuenteid && rra.ProductoId == productoId) {
                                rra.Metas.forEach(rec => {
                                    if (rec.Diferencia != 0) {
                                        var resumen = {
                                            Vigencia: rec.Vigencia,
                                            Departamento: rec.Departamento,
                                            Municipio: rec.Municipio,
                                            TipoAgrupacion: rec.TipoAgrupacion,
                                            Agrupacion: rec.Agrupacion,
                                            EnFirme: rec.EnFirme,
                                            EnAjuste: rec.EnAjuste,
                                            Diferencia: rec.Diferencia,
                                        };
                                        listafinal.push(resumen);
                                    }
                                });
                            }
                        });
                        break;
                    }
            }
            vm.listaResumen = listafinal;
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ConvertirNumero4decimales(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        vm.editar = function (estado) {
            vm.isEdicion = null;
            vm.isEdicion = estado == 'editar';
            if (vm.isEdicion) {
                document.getElementById("justificacionrecreg").disabled = false;
                document.getElementById("justificacionrecreg").classList.remove('disabled');
                document.getElementById("btn-guardar-edicion-ff").classList.remove('disabled');
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
            } else {
                document.getElementById("justificacionrecreg").disabled = true;
                document.getElementById("justificacionrecreg").classList.add('disabled');
                /*document.getElementById("btn-guardar-edicion-ff").classList.add('disabled');*/
                document.getElementById("justificacionrecreg").value = vm.justificacioncapitulo;
                vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
            }
        }

        vm.guardar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                utilidades.mensajeError('Debe ingresar una justificación.');
                return false;
            }
            ObtenerSeccionCapitulo();
            //var seccionCapitulo = document.getElementById("seccion-capitulo-recursosfuentesdefinanc");
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: vm.justificacion,
                SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
                //SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                AplicaJustificacion: 1,
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess(response.data.Mensaje);
                        document.getElementById("justificacionrecreg").value = vm.justificacion;
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
                vm.validacion = "Debe diligenciar la justificación del capítulo Recursos Regionalizacion dentro de la pestaña Justificación.";
                return false;
            }
            vm.validacion = "";
            return true;
        }

        vm.notificacionValidacion = function (errores) {
            console.log("Validación  - Justificación Recursos Regionalizacion");
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRecursosRegionalizacion = errores.find(p => p.Capitulo == vm.nombreComponente);
                if (erroresRecursosRegionalizacion != undefined) {
                    var erroresJson = erroresRecursosRegionalizacion.Errores == "" ? [] : JSON.parse(erroresRecursosRegionalizacion.Errores);
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
            var campoObligatorioJustificacion = document.getElementById("recursosregionalizacion-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("recursosregionalizacion-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }
        }

        vm.validarJustificacion = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("recursosregionalizacion-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("recursosregionalizacion-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'JUST001': vm.validarJustificacion
        }
    }

    angular.module('backbone').component('recursosregionalizacion', {
        templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/regionalizacion/recursosregionalizacion.html",
        controller: recursosregionalizacionController,
        controllerAs: "vm",
        bindings: {
            justificacioncapitulo: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();