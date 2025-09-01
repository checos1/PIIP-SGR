(function () {
    'use strict';

    indicadoresPoliticaSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'gestionRecursosSGPServicio',
        '$anchorScroll',
        '$location'
    ];

    function indicadoresPoliticaSgpController(
        $scope,
        $sessionStorage,
        gestionRecursosSGPServicio,
        $anchorScroll,
        $location
    ) {
        var vm = this;
        vm.lang = "es";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.nombreElementoOrigen = sessionStorage.getItem("nombreElementoOrigen");
        vm.politicaId = sessionStorage.getItem("politicaId");
        vm.nombreTipoIndEquidadMujer = 'Indicadores ODS y PND';
        vm.nombreTipoIndConstPaz = 'PMI (Plan Marco de Implementación)';
        $scope.datos = [];

        //Inicio
        vm.init = function () {
            vm.cargarCss('/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/indicadoresPoliticaSgp/indicadoresPoliticaSgp.css',            
                'indicadoresPoliticaSgpCSS');
            vm.obtenerDatosIP(vm.BPIN);
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            vm.obtenerDatosIP(vm.BPIN);
        }

        vm.scrollTo = function (id) {
            var old = $location.hash();
            $location.hash(id);
            //$anchorScroll.yOffset = 50;
            $anchorScroll();
            //reset to old to keep any additional routing logic from kicking in
            $location.hash(old);
        };

        vm.retornar = function () {
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

            vm.ocultarIndPolitica(e);
        };

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
                        var d = dRepeat.getElementsByClassName('dIndicadores');
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

                    //var dDescripcionInd = iDescripcionIndicador.nextElementSibling;

                    //if (iDescripcionIndicador.classList.contains("fa-plus-square")) {
                    //    if (dDescripcionInd) {
                    //        dDescripcionInd.style.display = 'none';
                    //    }
                    //}
                    //else if (iDescripcionIndicador.classList.contains("fa-minus-square")) {
                    //    if (dDescripcionInd) {
                    //        dDescripcionInd.style.display = 'block';
                    //    }
                    //}
                }
            }

            //var iDescripcionIndicador = document.getElementById('iDescripcionIndicador');
            //if (iDescripcionIndicador) {
            //    iDescripcionIndicador.classList.toggle("fa-plus-square");
            //    iDescripcionIndicador.classList.toggle("fa-minus-square");
            //}

            //if (iDescripcionIndicador.classList.contains("fa-plus-square")) {
            //    $("#dDescripcionIndicador").css("display", "none");
            //}
            //else if (iDescripcionIndicador.classList.contains("fa-minus-square")) {
            //    $("#dDescripcionIndicador").css("display", "block");
            //}
        };

        vm.recortarTexto = function (texto, longitud) {
            var totalLongitudTexto = texto.length;
            if (texto && totalLongitudTexto > longitud) {
                texto = texto.substring(0, longitud) + '...';
            }

            return texto;
        };

        vm.recortarTextoCategorias = function () {
            var dCategoriasIP = document.getElementById('dCategoriasIP');
            if (dCategoriasIP) {
                var p = dCategoriasIP.getElementsByTagName('span');
                if (p) {
                    for (var i = 0; i < p.length; i++) {
                        var p1 = p[i];
                        if (p1) {
                            var texto = p1.innerHTML;
                            if (texto) {
                                texto = vm.recortarTexto(texto, 80);
                                p1.innerHTML = texto ? texto : '';
                            }
                        }
                    }
                }
            }
        };

        vm.verMasDescripcionCategoria = function (element, nombre) {
            if (element) {
                var currentTarget = element.currentTarget;
                if (currentTarget) {
                    var parent = currentTarget.parentNode;
                    if (parent) {
                        var hd = parent.getElementsByTagName('input');
                        if (hd && hd.length > 0) {
                            if (nombre === 'Categoria 1:') {
                                var texto = hd[0].value;
                                $("#dModalIPContent").html(texto);
                            } else if (nombre === 'Categoria 2:') {
                                var texto2 = hd[1].value;
                                $("#dModalIPContent").html(texto2);
                            }

                            $("#IPModalTitle").html(nombre);
                        }
                    }
                }
            }
        };

        /// Obtener información de IP.
        vm.obtenerDatosIP = function (id) {
            //var idInstancia = $sessionStorage.idNivel;
            //var idAccion = $sessionStorage.idNivel;
            //var idFormulario = $sessionStorage.idNivel;
            //var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;                     

            gestionRecursosSGPServicio
                .obtenerDatosIndicadoresPoliticaSgp($sessionStorage.idInstancia)
                .then(
                    (respuesta) => {
                        if (respuesta.data != null && respuesta.data != "") {
                            var jResult = JSON.parse(respuesta.data);
                            if (jResult && typeof jResult === 'string') {
                                jResult = JSON.parse(jResult);
                            }

                            if (jResult.Politicas) {
                                var politica = jResult.Politicas.filter(e => e.PoliticaId.toString() === vm.politicaId.toString());
                                if (politica && politica.length > 0) {
                                    for (var i = 0; i < politica.length; i++) {
                                        var localizaciones = politica[i].Localizaciones;
                                        var localizacionesDistinct = [];
                                        var Politica = politica[i].Politica;
                                        var politicaId = politica[i].PoliticaId;
                                        if (localizaciones && localizaciones.length > 0) {
                                            for (var j = 0; j < localizaciones.length; j++) {
                                                var loc = localizaciones[j];
                                                var titulo = vm.tituloLocalizacion(localizaciones[j]);
                                                loc.titulo = titulo;

                                                var exist = localizacionesDistinct.filter(f =>
                                                    f.Localizacion.trim().toLowerCase() === loc.Localizacion.trim().toLowerCase());
                                                if (exist && exist.length === 0) {
                                                    localizacionesDistinct.push(loc);
                                                }
                                            }

                                            if (localizacionesDistinct.length > 0) {
                                                politica[i].Localizaciones = localizacionesDistinct;
                                            }
                                        }

                                        if (Politica) {
                                            politica[i].Politica = vm.capitalizarPrimeraLetra(Politica);
                                        }

                                        politica[i].TipoIndicador = '';

                                        if (politicaId === 4) {
                                            politica[i].TipoIndicador = vm.nombreTipoIndConstPaz;
                                        }

                                        if (politicaId === 7) {
                                            politica[i].TipoIndicador = vm.nombreTipoIndEquidadMujer;
                                        }
                                    }
                                }
                            }

                            $scope.datos = politica;
                            //setTimeout(() => vm.filtrarDatosResult(vm.politicaId), 500);
                        }
                    });
        };

        vm.cerrarModal = function (id) {
            $('#' + id).modal('hide');
        };

        vm.filtrarDatosResult = function (politicaId) {
            /// Filtrar vista de datos.
            var secIndicadoresPolitica = document.getElementById('secIndicadoresPolitica');
            if (secIndicadoresPolitica && politicaId) {
                var s = secIndicadoresPolitica.getElementsByClassName('cDivMainIndPolitica');
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

                    /// No tiene datos.
                    if (!encontro) {
                        var cMensajeIndPolitica = secIndicadoresPolitica.getElementsByClassName('cMensajeIndPolitica');
                        if (cMensajeIndPolitica) {
                            cMensajeIndPolitica[0].innerHTML = '¡Sin datos de consulta!.';
                        }
                    }
                }
            }
        };

        vm.ocultarIndPolitica = function (e) {
            var e2 = e.closest(".cDivMainIndPolitica");
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

        /// Poner el mayuscula la primera letra.
        vm.capitalizarPrimeraLetra = function (str) {
            str = str.toLowerCase();
            return str.charAt(0).toUpperCase() + str.slice(1);
        };


        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-recursosgrfocalizacion');
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

            gestionRecursosServicio.guardarCambiosFirme(data)
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
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            gestionRecursosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }
    }

    angular.module('backbone').component('indicadoresPoliticaSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/indicadoresPoliticaSgp/indicadoresPoliticaSgp.html",        
        controller: indicadoresPoliticaSgpController,
        controllerAs: "vm",
        bindings: {
            notificacioncambios: '&',
        }
    });
})();