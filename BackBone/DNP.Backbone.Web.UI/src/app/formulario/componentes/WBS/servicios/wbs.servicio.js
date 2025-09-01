(function () {
    'use strict';

    angular.module('backbone.formulario').factory('wbsServicio', wbsServicio);

    wbsServicio.$inject = [];

    function wbsServicio() {
        var datosArray = [];

        function tipoDeAtributo(obj) {
            return ({}).toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
        }

        function filtrarWBS(listaPropiedades, datos, filtros) {
            if (datos) {
                var ramas = obtenerListaRamas(listaPropiedades, datos);
                _.each(ramas, function (rama) {
                    var res = ramaPasaFiltros(rama, filtros);
                    if (res.pasoFiltros) {
                        var sonDescendientes = false;
                        for (var indiceElementoRama = 0; indiceElementoRama < rama.length; indiceElementoRama++) {
                            if (rama[indiceElementoRama] === res.ultimoElementoRamaFiltrado) {
                                rama[indiceElementoRama].Filtrar = true;
                                rama[indiceElementoRama].colapsarPanelWBS = true;
                                sonDescendientes = true;
                            } else {
                                if (sonDescendientes) {
                                    rama[indiceElementoRama].Filtrar = true;
                                    rama[indiceElementoRama].colapsarPanelWBS = true;
                                } else {
                                    //Son antecesores
                                    rama[indiceElementoRama].Filtrar = true;
                                    rama[indiceElementoRama].colapsarPanelWBS = false;
                                }
                            }
                        }
                    }
                });
            }
        }

        function collapsarDatos(datos) {

            if (datos !== null && typeof (datos) === 'object' && datos.hasOwnProperty("colapsarPanelWBS"))
                datos.colapsarPanelWBS = true;

            _.each(datos, function (propiedad) {

                if (Array.isArray(propiedad))
                    collapsarArray(propiedad);


                if (propiedad !== null && typeof (propiedad) === 'object' && propiedad.hasOwnProperty("colapsarPanelWBS"))
                    propiedad.colapsarPanelWBS = true;                                
            });
        }

        function collapsarArray(array) {
            array.forEach(x => collapsarDatos(x));
        }

        function ramaPasaFiltros(rama, filtros) {
            var cantidadFiltrosPasados = 0;
            var cantidadFiltrosSeleccionados = obtenerCantidadFiltrosSeleccionados(filtros);
            var ultimoElementoFiltrado = null;
            for (var indiceElemento = 0; indiceElemento < rama.length; indiceElemento++) {
                var filtrosEncontrados = filtros.filter(filtro => (filtro.elementosRuta.length === indiceElemento + 1 && filtro.valorSeleccionado !== undefined) || (filtro.elementosRuta.length === indiceElemento + 1 && filtro.valorSeleccionado === null));
                if (filtrosEncontrados.length > 0) {
                    _.each(filtrosEncontrados, function (filtroEncontrado) {
                        var nombrePropiedad = filtroEncontrado.elementosRuta[filtroEncontrado.elementosRuta.length - 1];
                        if (rama[indiceElemento][nombrePropiedad] === filtroEncontrado.valorSeleccionado || filtroEncontrado.valorSeleccionado === null) { //null es Todos
                            cantidadFiltrosPasados++;
                            ultimoElementoFiltrado = rama[indiceElemento];
                        }
                    });
                }
            }

            var resultado = {
                pasoFiltros: cantidadFiltrosPasados === cantidadFiltrosSeleccionados,
                ultimoElementoRamaFiltrado: ultimoElementoFiltrado
            }

            return resultado;
        }

        function obtenerListaRamas(listaPropiedades, dato, ramasEncontradas = [], rutaRamaEnCreacion = []) {
            if (dato) {
                if (Array.isArray(dato)) {
                    _.each(dato, function (elementoDato) {
                        obtenerRamaDesdeDato(listaPropiedades, elementoDato, ramasEncontradas, rutaRamaEnCreacion)
                    });
                } else {
                    obtenerRamaDesdeDato(listaPropiedades, dato, ramasEncontradas, rutaRamaEnCreacion)
                }
            }

            return ramasEncontradas;
        }

        function obtenerRamaDesdeDato(listaPropiedades, dato, ramasEncontradas, rutaRamaEnCreacion) {
            rutaRamaEnCreacion.push(dato);

            var propiedadesArray = listaPropiedades.filter(configuracionPropiedad => configuracionPropiedad.tipo === 'array');

            var cantidadPropiedadesArrayConElementos = 0;
            _.each(propiedadesArray, function (configuracionPropiedadArray) {
                if (dato[configuracionPropiedadArray.propiedad] !== null
                    && dato[configuracionPropiedadArray.propiedad] !== undefined
                    && Array.isArray(dato[configuracionPropiedadArray.propiedad])
                    && dato[configuracionPropiedadArray.propiedad].length > 0)
                    cantidadPropiedadesArrayConElementos++;
            });

            if (propiedadesArray.length === 0 || cantidadPropiedadesArrayConElementos === 0) {
                //Es hoja!
                var ramaEncontrada = [];

                _.each(rutaRamaEnCreacion, function (elementoRama) {
                    ramaEncontrada.push(elementoRama);
                });

                ramasEncontradas.push(ramaEncontrada);
            } else {
                _.each(propiedadesArray, function (configuracionPropiedadArray) {
                    obtenerListaRamas(configuracionPropiedadArray.items, dato[configuracionPropiedadArray.propiedad], ramasEncontradas, rutaRamaEnCreacion);
                });
            }

            rutaRamaEnCreacion.pop();
        }

        function obtenerRama(listaPropiedades, dato, elementosRutaActual, ramasEncontradas) {
            if (dato) {
                if (Array.isArray(dato)) {
                    _.each(dato, function (elementoDato) {

                        obtenerRama(listaPropiedades, elementoDato, elementosRutaActual, ramasEncontradas);

                        var rutaEncontrada = [];
                        _.each(elementosRutaActual, function (elementoRuta) {
                            rutaEncontrada.push(elementoRuta);
                        });

                        ramasEncontradas.push(rutaEncontrada);

                        elementosRutaActual.pop();
                    });
                } else {
                    elementosRutaActual.push(dato);

                    _.each(listaPropiedades, function (propiedad) {
                        if (propiedad.tipo === 'array' && dato[propiedad.propiedad] != undefined && Array.isArray(dato[propiedad.propiedad]) && dato[propiedad.propiedad].length > 0)
                            obtenerRama(propiedad.items, dato[propiedad.propiedad], elementosRutaActual, ramasEncontradas);
                    });
                }
            }
        }

        function obtenerCantidadFiltrosSeleccionados(filtros) {
            var cantidadFiltrosSeleccionados = 0;
            _.each(filtros, function (filtro) {
                if (filtro.valorSeleccionado !== undefined)//null igual a Todos
                    cantidadFiltrosSeleccionados++;
            });
            return cantidadFiltrosSeleccionados;
        }

        function construirFiltros(listaPropiedades, datos) {
            var filtros = crearFiltrosDesdeConfiguracion(listaPropiedades);
            filtros = llenarValoresFiltrosRecursivo(listaPropiedades, datos, filtros);
            return filtros;
        }

        function crearFiltrosDesdeConfiguracion(listaPropiedades, filtros = [], rutaFiltroEnCreacion = []) {
            _.each(listaPropiedades, function (configuracionPropiedad) {
                if (configuracionPropiedad.tipo === 'array') {
                    rutaFiltroEnCreacion.push(configuracionPropiedad.propiedad);
                    crearFiltrosDesdeConfiguracion(configuracionPropiedad.items, filtros, rutaFiltroEnCreacion);
                    rutaFiltroEnCreacion.pop();
                } else {
                    if (configuracionPropiedad.filtrar === true) {
                        rutaFiltroEnCreacion.push(configuracionPropiedad.propiedad);

                        var tempRutaFiltro = angular.copy(rutaFiltroEnCreacion);

                        var nuevoFiltro = {
                            ruta: tempRutaFiltro.join('-'),
                            elementosRuta: tempRutaFiltro,
                            nombre: _.last(tempRutaFiltro, 2).join('-'),
                            valores: []
                        };

                        filtros.push(nuevoFiltro);

                        rutaFiltroEnCreacion.pop();
                    }
                }
            });

            return filtros;
        }

        function llenarValoresFiltrosRecursivo(listaPropiedades, dato, filtros, rutaFiltroEnCreacion = []) {
            if (dato) {
                if (Array.isArray(dato)) {
                    _.each(dato, function (elementoDato) {
                        llenarFiltrosDesdeDato(listaPropiedades, elementoDato, filtros, rutaFiltroEnCreacion)
                    });
                } else {
                    llenarFiltrosDesdeDato(listaPropiedades, dato, filtros, rutaFiltroEnCreacion)
                }
            }

            return filtros;
        }

        function llenarFiltrosDesdeDato(listaPropiedades, dato, filtros, rutaFiltroEnCreacion) {
            _.each(listaPropiedades, function (propiedad) {
                if (propiedad.tipo !== 'array') {
                    if (propiedad.filtrar === true) {

                        rutaFiltroEnCreacion.push(propiedad.propiedad);

                        var rutaFiltroEnCreacionConcatenda = rutaFiltroEnCreacion.join('-');

                        var rutaExistente = filtros.find(function (rutaExistente) {
                            return rutaExistente.ruta === rutaFiltroEnCreacionConcatenda;
                        });

                        if (rutaExistente) {
                            if (dato[propiedad.propiedad] !== undefined && dato[propiedad.propiedad] !== null && dato[propiedad.propiedad] !== "") {
                                var valorExistente = rutaExistente.valores.find(function (valor) {
                                    return valor === dato[propiedad.propiedad];
                                });

                                if (valorExistente === undefined)
                                    rutaExistente.valores.push(dato[propiedad.propiedad]);
                            }
                        }

                        rutaFiltroEnCreacion.pop();
                    }
                } else {
                    //Es un objeto con multiple propiedades!
                    rutaFiltroEnCreacion.push(propiedad.propiedad);
                    llenarValoresFiltrosRecursivo(propiedad.items, dato[propiedad.propiedad], filtros, rutaFiltroEnCreacion);
                    rutaFiltroEnCreacion.pop(propiedad.propiedad);
                }
            });
        }

        //Ocultar datos permite  agregar nuevos atributos a los nodos y reiniciar el arbol completo ocultando todos sus nodos

        function ocultarDatosCompleto(datos) {
            inicializarWBS(datos, false);
        }

        function inicializarWBS(datos, mostrarTodo = true) {
            if (datos)
                if (Object.keys(datos).length > 0) {
                    if (Array.isArray(datos)) {
                        inicializarArrayWBS(datos, mostrarTodo);
                    } else {
                        inicializarDatoWBS(datos, mostrarTodo);
                    }
                }
        }

        function inicializarDatoWBS(dato, mostrarTodo = true) {
            if (!Array.isArray(dato) && Object.keys(dato).length > 0) {
                if (dato.hasOwnProperty("Filtrar")) {
                    dato.Filtrar = mostrarTodo;
                } else if (typeof (dato) === 'object' && dato !== null) {
                    dato.Filtrar = mostrarTodo;
                }

                if (dato.hasOwnProperty("colapsarPanelWBS")) {
                    dato.colapsarPanelWBS = true;
                } else if(typeof(dato) === 'object' && dato !== null){
                    dato.colapsarPanelWBS = true;
                }

                _.each(dato, function (propiedad) {
                    if (Array.isArray(propiedad)) {
                        
                        inicializarArrayWBS(propiedad, mostrarTodo);
                    }
                });
            }
        }

        function inicializarArrayWBS(array, mostrarTodo) {
            _.each(array, function (elemento) {
                
                if (!Array.isArray(elemento)) {
                    
                    inicializarDatoWBS(elemento, mostrarTodo);
                }
            });
        }

        return {
            tipoDeAtributo: tipoDeAtributo,
            filtrarWBS: filtrarWBS,
            ocultarDatosCompleto: ocultarDatosCompleto,
            construirFiltros: construirFiltros,
            inicializarWBS: inicializarWBS,
            collapsarDatos: collapsarDatos
        }
    }
})();