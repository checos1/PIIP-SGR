(function () {
    'use strict';

    focalizacioncategoriaspolitController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'justificacionCambiosServicio',
        'constantesBackbone',
        'focalizacioncategoriaspolitServicio'
    ];

    function focalizacioncategoriaspolitController($scope,
        $sessionStorage,
        utilidades,
        justificacionCambiosServicio,
        constantesBackbone,
        focalizacioncategoriaspolitServicio
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

        vm.init = function () {

            obtenerDetalleAjustesJustificaionFacalizacionPT();
            setTimeout(function () {
                obtenerSeccionOtrasPoliticasFacalizacionPT()
            }, 10000
            )
            ObtenerSeccionPoliticasDT();
            vm.editar();
            ObtenerSeccionCapitulo();
            vm.justificacion = vm.justificacioncapitulo;
            vm.notificacioncambios({ handler: vm.notificacionJustificacion });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });

        };

        function ObtenerSeccionPoliticasDT() {
            return focalizacioncategoriaspolitServicio.ObtenerSeccionPoliticaFocalizacionDT(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.SeccionPoliticasDT = jQuery.parseJSON(arreglolistas);
                        vm.SeccionPoliticasDTBK = jQuery.parseJSON(arreglolistas);

                        var origen = 0;
                        var i = 0;
                        var verpolitica = 0;
                        vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                            if (poli.DetallePoliticas != null) {
                                poli.DetallePoliticas.forEach(polipol => {
                                    origen = 0;
                                    if (polipol.Recursos != null || polipol.Metas != null || polipol.Personas != null || polipol.MetasSecundarias != null) {
                                        polipol.Visible = 1;
                                        poli.justificacion = "";
                                        if (polipol.Recursos != null) {
                                            origen = 1;
                                        }
                                        else if (polipol.Metas != null) {
                                            origen = 2;
                                        }
                                        else if (polipol.Personas != null) {
                                            origen = 3;
                                        }
                                        else if (polipol.MetasSecundarias != null) {
                                            origen = 4;
                                        }
                                        verpolitica = 1;
                                        vm.mostrarTabDT(origen, poli.PoliticaId, polipol.CategoriaId, polipol.ProductoId, i);
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
                            if (poli.DetalleAjuste != null) {
                                poli.DetalleAjuste.forEach(deta => {
                                    if (deta.Tipo == "Agregado") {
                                        verpolitica = 1;
                                        poli.VerPolitica = verpolitica;
                                        deta.Detalle = "Se agrego la politica: " + poli.Politica;
                                    }
                                    else if (deta.Tipo == "Nuevo") {
                                        deta.Detalle = "Se agrego la categoria: " + deta.Categoria + " y subcategoria: " + deta.SubCategoria;
                                    }
                                    else if (deta.Tipo == "Modificado") {
                                        deta.Detalle = "";
                                    }
                                    else {
                                        deta.Detalle = "Se desagrego la categoria: " + deta.Categoria + " y subcategoria: " + deta.SubCategoria;
                                    }

                                });
                            }
                        });
                    }
                });
        }

        function obtenerDetalleAjustesJustificaionFacalizacionPT() {
            return focalizacioncategoriaspolitServicio.obtenerDetalleAjustesJustificaionFacalizacionPT(vm.BPIN, vm.idUsuario).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.DetalleAjustes = respuesta.data.split('|');
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

        function obtenerSeccionOtrasPoliticasFacalizacionPT() {
            return focalizacioncategoriaspolitServicio.obtenerSeccionOtrasPoliticasFacalizacionPT(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.AjustesJustificaionRegionalizacion = jQuery.parseJSON(arreglolistas);

                        var origen = 0;
                        var i = 0;
                        var verpolitica = 0;
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

        vm.AbrilNivelDT = function () {

            var variable = $("#ico-otras-politicasDT")[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-otras-politicasDT");
            var imgmenos = document.getElementById("imgmenos-otras-politicasDT");
            if (variable === "+") {
                $("#ico-otras-politicasDT").html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-otras-politicasDT").html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.AbrilNivel1DT = function (politicaId, indexpoliticas) {

            var variable = $("#icodt-" + politicaId + "-" + indexpoliticas)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasdt-" + politicaId + "-" + indexpoliticas);
            var imgmenos = document.getElementById("imgmenosdt-" + politicaId + "-" + indexpoliticas);
            if (variable === "+") {
                $("#icodt-" + politicaId + "-" + indexpoliticas).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icodt-" + politicaId + "-" + indexpoliticas).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            var origen = 0;
            var i = 0;
            var verpolitica = 0;
            vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                poli.Detalle = [];
                if (poli.DetallePoliticas != null) {
                    i = 0;
                    poli.DetallePoliticas.forEach(polipol => {
                        origen = 0;
                        if (polipol.Recursos != null || polipol.Metas != null || polipol.Personas != null || polipol.MetasSecundarias != null) {
                            polipol.Visible = 1;
                            poli.justificacion = "";
                            if (polipol.Recursos != null) {
                                origen = 1;
                            }
                            else if (polipol.Metas != null) {
                                origen = 2;
                            }
                            else if (polipol.Personas != null) {
                                origen = 3;
                            }
                            else if (polipol.MetasSecundarias != null) {
                                origen = 4;
                            }
                            verpolitica = 1;
                            vm.mostrarTabDT(origen, poli.PoliticaId, polipol.CategoriaId, polipol.ProductoId, i);
                            i++;
                        }
                        else {
                            polipol.Visible = 0;
                        }
                    });
                }
                poli.VerPolitica = verpolitica;
                verpolitica = 0;

                if (poli.DetalleAjuste != null) {
                    poli.DetalleAjuste.forEach(deta => {
                        if (deta.Tipo == "Agregado") {
                            verpolitica = 1;
                            poli.VerPolitica = verpolitica;
                            poli.Detalle.push("Se agrego la politica: " + poli.Politica);
                        }
                        else if (deta.Tipo == "Nuevo") {
                            deta.Detalle = "Se agrego la categoria: " + deta.Categoria + " y subcategoria: " + deta.SubCategoria;
                            poli.Detalle.push("Se agrego la categoria: " + deta.Categoria + " y subcategoria: " + deta.SubCategoria);
                        }
                        else if (deta.Tipo == "Modificado") {
                            deta.Detalle = "";
                        }
                        else {
                            deta.Detalle = "Se desagrego la categoria: " + deta.Categoria + " y subcategoria: " + deta.SubCategoria;
                            poli.Detalle.push("Se desagrego la categoria: " + deta.Categoria + " y subcategoria: " + deta.SubCategoria);
                        }

                    });
                }
            });
        }

        vm.mostrarTabDT = function (origen, politicaId, categoriaId, productoId, indexpoliticas) {

            vm.listaResumen;
            var politica = "";

            var ValorFirme = 0;
            var ValorAjuste = 0;
            var ValorDiferencia = 0;


            vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                if (poli.DetallePoliticas != null) {
                    poli.DetallePoliticas.forEach(detalle => {
                        if (poli.PoliticaId == politicaId && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                            politica = poli;
                        }
                    });
                }
            });

            var recursostable = document.getElementById("recursosdt" + politicaId + "-" + categoriaId + "-" + productoId + "-" + indexpoliticas);
            var metastable = document.getElementById("metasdt" + politicaId + "-" + categoriaId + "-" + productoId + "-" + indexpoliticas);
            var personastable = document.getElementById("personasdt" + politicaId + "-" + categoriaId + "-" + productoId + "-" + indexpoliticas);
            var metassecundariastable = document.getElementById("metassecundariasdt" + politicaId + "-" + categoriaId + "-" + productoId + "-" + indexpoliticas);

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
                        if (metassecundariastable != undefined) {
                            metassecundariastable.classList.add('hidden');
                        }

                        ValorFirme = 0;
                        ValorAjuste = 0;
                        ValorDiferencia = 0;

                        politica.DetallePoliticas.forEach(detalle => {
                            if (detalle.Recursos != null && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                detalle.Recursos.forEach(item => {
                                    ValorFirme += parseFloat(item.ValorFirme);
                                    ValorAjuste += parseFloat(item.ValorActual);
                                    ValorDiferencia += parseFloat(item.Diferencia);
                                });
                            }
                        });

                        vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                            if (poli.DetallePoliticas != null) {
                                poli.DetallePoliticas.forEach(detalle => {
                                    if (poli.PoliticaId == politicaId && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                        if (detalle.Recursos != null) {
                                            detalle.Recursos.ValorFirme = ValorFirme;
                                            detalle.Recursos.ValorAjuste = ValorAjuste;
                                            detalle.Recursos.ValorDiferencia = ValorDiferencia;
                                        }
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
                        if (personastable != undefined) {
                            personastable.classList.add('hidden');
                        }
                        if (metassecundariastable != undefined) {
                            metassecundariastable.classList.add('hidden');
                        }

                        ValorFirme = 0;
                        ValorAjuste = 0;
                        ValorDiferencia = 0;

                        politica.DetallePoliticas.forEach(detalle => {
                            if (detalle.Metas != null && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                detalle.Metas.forEach(item => {
                                    ValorFirme += parseFloat(item.MetaFirme);
                                    ValorAjuste += parseFloat(item.MetaActual);
                                    ValorDiferencia += parseFloat(item.Diferencia);
                                });
                            }
                        });

                        vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                            if (poli.DetallePoliticas != null) {
                                poli.DetallePoliticas.forEach(detalle => {
                                    if (poli.PoliticaId == politicaId && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                        if (detalle.Metas != null) {
                                            detalle.Metas.ValorFirme = ValorFirme;
                                            detalle.Metas.ValorAjuste = ValorAjuste;
                                            detalle.Metas.ValorDiferencia = ValorDiferencia;
                                        }
                                    }
                                });
                            }
                        });

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
                        if (metassecundariastable != undefined) {
                            metassecundariastable.classList.add('hidden');
                        }

                        ValorFirme = 0;
                        ValorAjuste = 0;
                        ValorDiferencia = 0;

                        politica.DetallePoliticas.forEach(detalle => {
                            if (detalle.Personas != null && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                detalle.Personas.forEach(item => {
                                    ValorFirme += parseFloat(item.PersonasFirmeD);
                                    ValorAjuste += parseFloat(item.PersonasActualD);
                                    ValorDiferencia += parseFloat(item.DiferenciaD);                                                                      
                                });
                            }
                        });

                        vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                            if (poli.DetallePoliticas != null) {
                                poli.DetallePoliticas.forEach(detalle => {
                                    if (poli.PoliticaId == politicaId && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                        if (detalle.Personas != null) {
                                            detalle.Personas.ValorFirme = ConvertirNumero(ValorFirme);
                                            detalle.Personas.ValorAjuste = ConvertirNumero(ValorAjuste);
                                            detalle.Personas.ValorDiferencia = ConvertirNumero(ValorDiferencia);
                                        }
                                    }
                                });
                            }
                        });

                        break;
                    }
                case 4:
                    {
                        if (metassecundariastable != undefined) {
                            metassecundariastable.classList.remove('hidden');
                        }
                        if (recursostable != undefined) {
                            recursostable.classList.add('hidden');
                        }
                        if (metastable != undefined) {
                            metastable.classList.add('hidden');
                        }
                        if (personastable != undefined) {
                            personastable.classList.add('hidden');
                        }

                        politica.DetallePoliticas.forEach(detalle => {
                            if (detalle.MetasSecundarias != null && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                detalle.MetasSecundarias.forEach(item => {
                                    ValorFirme += parseFloat(item.MetaIndicadorSecundarioFirme);
                                    ValorAjuste += parseFloat(item.MetaIndicadorSecundarioActual);
                                    ValorDiferencia += parseFloat(item.Diferencia);
                                });
                            }
                        });

                        vm.SeccionPoliticasDT.Politicas.forEach(poli => {
                            if (poli.DetallePoliticas != null) {
                                poli.DetallePoliticas.forEach(detalle => {
                                    if (poli.PoliticaId == politicaId && detalle.CategoriaId == categoriaId && detalle.ProductoId == productoId) {
                                        if (detalle.MetasSecundarias != null) {
                                            detalle.MetasSecundarias.ValorFirme = ValorFirme;
                                            detalle.MetasSecundarias.ValorAjuste = ValorAjuste;
                                            detalle.MetasSecundarias.ValorDiferencia = ValorDiferencia;
                                        }
                                    }
                                });
                            }
                        });

                        break;
                    }
            }
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

        vm.editarDT = function (estado, politicaId) {
            switch (estado) {
                case "editar":
                    document.getElementById("btnCancelarPolitca-" + politicaId).classList.remove('hidden');
                    document.getElementById("btnEditarPolitca-" + politicaId).classList.add('hidden');
                    document.getElementById("justificacionfocapdt-" + politicaId).disabled = false;
                    document.getElementById("justificacionfocapdt-" + politicaId).classList.remove('disabled');
                    document.getElementById("btnGuardarPolitica-" + politicaId).classList.remove('disabled');
                    break;
                case "cancelar":
                    document.getElementById("btnEditarPolitca-" + politicaId).classList.remove('hidden');
                    document.getElementById("btnCancelarPolitca-" + politicaId).classList.add('hidden');
                    document.getElementById("justificacionfocapdt-" + politicaId).classList.add('disabled');
                    document.getElementById("btnGuardarPolitica-" + politicaId).classList.add('disabled');

                    var justificacion = "";
                    vm.SeccionPoliticasDTBK.Politicas.forEach(poli => {
                        if (poli.PoliticaId == politicaId) {
                            justificacion = poli.Justificacion;
                        }
                    });

                    document.getElementById("justificacionfocapdt-" + politicaId).value = justificacion;


                    break;
                default:
            }
        }

        vm.guardarDT = function (politicaId) {
            var justificacion = document.getElementById("justificacionfocapdt-" + politicaId).value;
            if (justificacion == '' || justificacion == undefined) {
                utilidades.mensajeError('Debe ingresar una justificación.');

                return false;
            }

            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: justificacion,
                SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                PoliticaId: politicaId,
            }

            justificacionCambiosServicio.FocalizacionActualizaPoliticasModificadas(data).then(function (response) {
                if (response.statusText === "OK" || response.status === 200) {
                    utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                    vm.editarDT("cancelar", politicaId);
                    document.getElementById("justificacionfocapdt-" + politicaId).value = justificacion;
                }
                else {
                    utilidades.mensajeError(response.data.Mensaje);
                }
            });

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

        vm.guardar = function () {
            if (vm.justificacion == '' || vm.justificacion == undefined) {
                utilidades.mensajeError('Debe ingresar una justificación.');
                return false;
            }

            //var seccionCapitulo = document.getElementById("seccion-capitulo-recursosfuentesdefinanc");
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: vm.justificacion,
                SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
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
                vm.validacion = "Debe diligenciar la justificación del capítulo Recursos Regionalizacion dentro de la pestaña Justificación.";
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
                var erroresfocalizacionpt = errores.find(p => p.Capitulo == "focalizacionpolresumendefocali");
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
            var campoObligatorioJustificacion = document.getElementById("focalizacionregionalizacion-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("focalizacionregionalizacion-justificacion-error-mns");
            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion4 = document.getElementById("focalizacionregionalizacion-justificacion-error-4");
            var ValidacionFFR1Error4 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-4");
            if (campoObligatorioJustificacion4 != undefined) {
                if (ValidacionFFR1Error4 != undefined) {
                    ValidacionFFR1Error4.innerHTML = '';
                    campoObligatorioJustificacion4.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion7 = document.getElementById("focalizacionregionalizacion-justificacion-error-7");
            var ValidacionFFR1Error7 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-7");
            if (campoObligatorioJustificacion7 != undefined) {
                if (ValidacionFFR1Error7 != undefined) {
                    ValidacionFFR1Error7.innerHTML = '';
                    campoObligatorioJustificacion7.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion9 = document.getElementById("focalizacionregionalizacion-justificacion-error-9");
            var ValidacionFFR1Error9 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-9");
            if (campoObligatorioJustificacion9 != undefined) {
                if (ValidacionFFR1Error9 != undefined) {
                    ValidacionFFR1Error9.innerHTML = '';
                    campoObligatorioJustificacion9.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion10 = document.getElementById("focalizacionregionalizacion-justificacion-error-10");
            var ValidacionFFR1Error10 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-10");
            if (campoObligatorioJustificacion10 != undefined) {
                if (ValidacionFFR1Error10 != undefined) {
                    ValidacionFFR1Error10.innerHTML = '';
                    campoObligatorioJustificacion10.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion11 = document.getElementById("focalizacionregionalizacion-justificacion-error-11");
            var ValidacionFFR1Error11 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-11");
            if (campoObligatorioJustificacion11 != undefined) {
                if (ValidacionFFR1Error11 != undefined) {
                    ValidacionFFR1Error11.innerHTML = '';
                    campoObligatorioJustificacion11.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion12 = document.getElementById("focalizacionregionalizacion-justificacion-error-12");
            var ValidacionFFR1Error12 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-12");
            if (campoObligatorioJustificacion12 != undefined) {
                if (ValidacionFFR1Error12 != undefined) {
                    ValidacionFFR1Error12.innerHTML = '';
                    campoObligatorioJustificacion12.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion13 = document.getElementById("focalizacionregionalizacion-justificacion-error-13");
            var ValidacionFFR1Error13 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-13");
            if (campoObligatorioJustificacion13 != undefined) {
                if (ValidacionFFR1Error13 != undefined) {
                    ValidacionFFR1Error13.innerHTML = '';
                    campoObligatorioJustificacion13.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion14 = document.getElementById("focalizacionregionalizacion-justificacion-error-14");
            var ValidacionFFR1Error14 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-14");
            if (campoObligatorioJustificacion14 != undefined) {
                if (ValidacionFFR1Error14 != undefined) {
                    ValidacionFFR1Error14.innerHTML = '';
                    campoObligatorioJustificacion14.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion20 = document.getElementById("focalizacionregionalizacion-justificacion-error-20");
            var ValidacionFFR1Error20 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-20");
            if (campoObligatorioJustificacion20 != undefined) {
                if (ValidacionFFR1Error20 != undefined) {
                    ValidacionFFR1Error20.innerHTML = '';
                    campoObligatorioJustificacion20.classList.add('hidden');
                }
            }

        }

        vm.validarJustificacion = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("focalizacionregionalizacion-justificacion-error");
            var ValidacionFFR1Error = document.getElementById("focalizacionregionalizacion-justificacion-error-mns");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionConsPaz = function (errores) {
            var campoObligatorioJustificacion4 = document.getElementById("focalizacionregionalizacion-justificacion-error-4");
            var ValidacionFFR1Error4 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-4");

            if (campoObligatorioJustificacion4 != undefined) {
                if (ValidacionFFR1Error4 != undefined) {
                    ValidacionFFR1Error4.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion4.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionEquidadMujer = function (errores) {
            var campoObligatorioJustificacion7 = document.getElementById("focalizacionregionalizacion-justificacion-error-7");
            var ValidacionFFR1Error7 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-7");

            if (campoObligatorioJustificacion7 != undefined) {
                if (ValidacionFFR1Error7 != undefined) {
                    ValidacionFFR1Error7.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion7.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionGEca = function (errores) {
            var campoObligatorioJustificacion9 = document.getElementById("focalizacionregionalizacion-justificacion-error-9");
            var ValidacionFFR1Error9 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-9");

            if (campoObligatorioJustificacion9 != undefined) {
                if (ValidacionFFR1Error9 != undefined) {
                    ValidacionFFR1Error9.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion9.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionGEci = function (errores) {
            var campoObligatorioJustificacion10 = document.getElementById("focalizacionregionalizacion-justificacion-error-10");
            var ValidacionFFR1Error10 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-10");

            if (campoObligatorioJustificacion10 != undefined) {
                if (ValidacionFFR1Error10 != undefined) {
                    ValidacionFFR1Error10.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion10.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionGEcn = function (errores) {
            var campoObligatorioJustificacion11 = document.getElementById("focalizacionregionalizacion-justificacion-error-11");
            var ValidacionFFR1Error11 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-11");

            if (campoObligatorioJustificacion11 != undefined) {
                if (ValidacionFFR1Error11 != undefined) {
                    ValidacionFFR1Error11.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion11.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionGEcp = function (errores) {
            var campoObligatorioJustificacion12 = document.getElementById("focalizacionregionalizacion-justificacion-error-12");
            var ValidacionFFR1Error12 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-12");

            if (campoObligatorioJustificacion12 != undefined) {
                if (ValidacionFFR1Error12 != undefined) {
                    ValidacionFFR1Error12.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion12.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionGRcr = function (errores) {
            var campoObligatorioJustificacion13 = document.getElementById("focalizacionregionalizacion-justificacion-error-13");
            var ValidacionFFR1Error13 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-13");

            if (campoObligatorioJustificacion13 != undefined) {
                if (ValidacionFFR1Error13 != undefined) {
                    ValidacionFFR1Error13.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion13.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionGEpr = function (errores) {
            var campoObligatorioJustificacion14 = document.getElementById("focalizacionregionalizacion-justificacion-error-14");
            var ValidacionFFR1Error14 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-14");

            if (campoObligatorioJustificacion14 != undefined) {
                if (ValidacionFFR1Error14 != undefined) {
                    ValidacionFFR1Error14.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion14.classList.remove('hidden');
                }
            }
        }

        vm.validarJustificacionVictimas = function (errores) {
            var campoObligatorioJustificacion20 = document.getElementById("focalizacionregionalizacion-justificacion-error-20");
            var ValidacionFFR1Error20 = document.getElementById("focalizacionregionalizacion-justificacion-error-mns-20");

            if (campoObligatorioJustificacion20 != undefined) {
                if (ValidacionFFR1Error20 != undefined) {
                    ValidacionFFR1Error20.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion20.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'JUST001': vm.validarJustificacion,
            'JUST4': vm.validarJustificacionConsPaz,
            'JUST7': vm.validarJustificacionEquidadMujer,
            'JUST9': vm.validarJustificacionGEca,
            'JUST10': vm.validarJustificacionGEci,
            'JUST11': vm.validarJustificacionGEcn,
            'JUST12': vm.validarJustificacionGEcp,
            'JUST13': vm.validarJustificacionGRcr,
            'JUST14': vm.validarJustificacionGEpr,
            'JUST20': vm.validarJustificacionVictimas,

        }
    }

    angular.module('backbone').component('focalizacioncategoriaspolit', {
        templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/focalizacion/categoriaspoliticastransversales/focalizacioncategoriaspolit.html",
        controller: focalizacioncategoriaspolitController,
        controllerAs: "vm",
        bindings: {
            justificacioncapitulo: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();