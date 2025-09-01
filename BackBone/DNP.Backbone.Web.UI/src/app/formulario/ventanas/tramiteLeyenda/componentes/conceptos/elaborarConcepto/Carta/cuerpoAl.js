(function () {
    'use strict';

    cuerpoAl.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'cartaServicio',
        'servicioAcciones',
        'justificacionCambiosServicio',
    ];

    function cuerpoAl(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        cartaServicio,
        servicioAcciones,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.conceptoValor = 'Concepto favorable';
        vm.establecido = 'establecido en';
        vm.nombreSccion = 'Cuerpo';
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.entidad = $sessionStorage.nombreProyecto != undefined ? $sessionStorage.nombreProyecto : $sessionStorage.usuario.permisos.Entidades[0].NombreEntidad;
        vm.fechaFinalVFO = $sessionStorage.anioFinalTramite != undefined ? $sessionStorage.anioFinalTramite : 0;
        vm.nombreComponente = 'conceptoselaborartecnico';
        vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY");
        vm.numeroRadicado = 0;
        obtenerFechaSinHoras(new Date());

        vm.disabled = true;
        vm.Editar = 'EDITAR';
        vm.heigprograma = 150;
        vm.mostrarCDP = true;
        vm.mostrarCRP = false;

        /*variables objetos*/

        vm.plantillaCarta = {};
        vm.carta = {};
        vm.listaCampos = [];
        vm.listaCRP = [];
        vm.datosProyectoAutorizacion = {};

        /* Metodos **/
        vm.guardarCuerpoConcepto = guardarCuerpoConcepto;

        vm.habilitaBotones = !$sessionStorage.soloLectura;

        function init() {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                    vm.tramiteId = vm.tramiteid;
                    vm.tipoTramite = vm.tipotramiteid;
                    vm.abrirpanel();
                    //consultarCDP();
                    consultarAutorizacion();
                    obtenerPlantillaCarta();
                }
            });

            
        }

        vm.abrirpanel = function () {
            var panel = document.getElementById('panelcuerpoconcepto');;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = "4000px";
            }
        }

        function obtenerPlantillaCarta() {
            cartaServicio.obtenerPlantillaCarta(vm.nombreSccion, vm.tipoTramite)
                .then(resultado => {
                    if (resultado != undefined && resultado.data) {
                        var PlantillaSecciones = [];
                        if (resultado.data.PlantillaSecciones != undefined) {
                            var seccion = {};
                            resultado.data.PlantillaSecciones.map(function (item) {
                                seccion.Id = item.Id;
                                seccion.PlantillaCartaId = item.PlantillaCartaId;
                                seccion.PlantillaSeccionCampos = [];
                                item.PlantillaSeccionCampos.map(function (itemCampo) {
                                    var campo = {};
                                    campo.Id = itemCampo.Id;
                                    campo.PlantillaCartaSeccionId = itemCampo.PlantillaCartaSeccionId;
                                    campo.NombreCampo = itemCampo.NombreCampo;
                                    campo.TipoCampo = itemCampo.TipoCampo;
                                    campo.TituloCampo = itemCampo.TituloCampo;
                                    campo.TextoDefecto = itemCampo.TextoDefecto;
                                    campo.Editable = itemCampo.Editable;
                                    campo.Orden = itemCampo.Orden;
                                    seccion.PlantillaSeccionCampos.push(campo);
                                    vm.listaCampos.push(campo);

                                });
                            });
                            PlantillaSecciones.push(seccion);
                        }


                        vm.plantillaCarta.Id = resultado.data.Id;
                        vm.plantillaCarta.IipoTramiteId = resultado.data.IipoTramiteId;
                        vm.plantillaCarta.PlantillaSecciones = PlantillaSecciones;
                        obtenerDatosIniciales();
                    }
                    else {
                        vm.plantillaCarta = {};
                    }

                });
        }

        function obtenerDatosIniciales() {
            var idPlantillaSeccion = vm.plantillaCarta.PlantillaSecciones[0].Id;
            cartaServicio.obtenerDatosCartaPorSeccion(vm.tramiteId, idPlantillaSeccion)
                .then(resultado => {
                    if (resultado != undefined && resultado.data != null && resultado.data.length > 0) {
                        var tmp = resultado.data[0];
                        vm.carta.Id = tmp.Id;
                        vm.carta.TramiteId = tmp.TramiteId;
                        vm.carta.Proceso = tmp.Proceso;
                        vm.carta.Entidad = tmp.Entidad;
                        vm.entidad = vm.carta.Entidad;
                        vm.carta.TipoId = vm.tipoTramite;
                        vm.numeroRadicado = tmp.RadicadoEntrada;
                        var cartaSeccion = [];
                        if (tmp.ListaCartaSecciones != undefined && tmp.ListaCartaSecciones.length > 0) {
                            var plantillaseccion = {};
                            tmp.ListaCartaSecciones.map(function (item) {
                                plantillaseccion = {
                                    Id: item.Id,
                                    CartaId: item.CartaId,
                                    ListaCartaCampos: vm.listaCampos.map(function (iteml) {
                                        var campo = {};
                                        var campotmp = item.ListaCartaCampos.find(x => x.NombreCampo === iteml.NombreCampo);
                                        if (campotmp) {
                                            campo.Id = campotmp.Id;
                                            //campo.Orden = campotmp.Orden;
                                            campo.CartaConceptoSeccionId = campotmp.CartaConceptoSeccionId;
                                            campo.PlantillaCartaCampoId = campotmp.PlantillaCartaCampoId;
                                            campo.DatoValor = campotmp.DatoValor !== undefined && campotmp.DatoValor !== '' ? campotmp.DatoValor : iteml.TextoDefecto;
                                            campo.NombreCampo = campotmp.NombreCampo;
                                            campo.Longitud = campotmp.TipoCampo.Longitud;
                                            campo.Tipo = campotmp.TipoCampo.Tipo;
                                            campo.ValorDefecto = '';
                                        }
                                        else {
                                            campo.Id = iteml.Id;
                                            //campo.Orden = campotmp.Orden;
                                            campo.CartaConceptoSeccionId = iteml.CartaConceptoSeccionId;
                                            campo.PlantillaCartaCampoId = iteml.PlantillaCartaCampoId;
                                            campo.NombreCampo = iteml.NombreCampo;
                                            campo.Longitud = iteml.TipoCampo.Longitud;
                                            campo.Tipo = iteml.TipoCampo.Tipo;
                                            campo.ValorDefecto = item.ValorDefecto;
                                            campo.DatoValor = item.ValorDefecto;
                                        }
                                        if (iteml.NombreCampo === 'OcultaCDP') {
                                            campo.DatoValor = campo.DatoValor === 'true' ? true : false;
                                        }

                                        return campo;
                                    })
                                }

                                cartaSeccion.push(plantillaseccion);

                            });


                        }

                        vm.carta.ListaCartaSecciones = cartaSeccion;

                    }
                    else {
                        vm.carta.Id = 0;
                        vm.carta.TramiteId = vm.tramiteId;
                        vm.carta.Proceso = vm.numeroTramite;
                        vm.carta.Entidad = vm.entidad;
                        vm.carta.TipoId = vm.tipoTramite;
                        var cartaSeccion = [];

                        var plantillaseccion = {
                            Id: vm.plantillaCarta.PlantillaSecciones[0].Id,
                            ListaCartaCampos: vm.listaCampos.map(function (iteml) {
                                var campo = {};

                                campo.Id = 0;
                                //campo.Orden = campotmp.Orden;
                                campo.CartaConceptoSeccionId = iteml.CartaConceptoSeccionId;
                                campo.PlantillaCartaCampoId = iteml.Id;
                                campo.NombreCampo = iteml.NombreCampo;
                                campo.Longitud = iteml.TipoCampo.Longitud;
                                campo.Tipo = iteml.TipoCampo.Tipo;
                                campo.ValorDefecto = iteml.ValorDefecto;
                                campo.DatoValor = iteml.TextoDefecto;

                                if (iteml.NombreCampo === 'OcultaCDP') {
                                    campo.DatoValor = iteml.TextoDefecto === 'true' ? true : false;
                                }
                                return campo;
                            })

                        }
                        cartaSeccion.push(plantillaseccion);
                        vm.carta.ListaCartaSecciones = cartaSeccion;

                    }

                    var texto2 = document.getElementById('texto2view');
                    texto2.innerHTML = vm.carta.ListaCartaSecciones[0].ListaCartaCampos[8].DatoValor;
                    var texto3 = document.getElementById('texto3view');
                    texto3.innerHTML = vm.carta.ListaCartaSecciones[0].ListaCartaCampos[10].DatoValor;
                });


        }

        function guardarCuerpoConcepto() {
            var parametros = vm.carta;
            if (parametros.ListaCartaSecciones[0].ListaCartaCampos[8].DatoValor.length > 8000) {
                swal('', "El Texto 2 es muy extenso", 'error');
            }
            else {
                if (parametros.ListaCartaSecciones[0].ListaCartaCampos[10].DatoValor.length > 8000) {
                    swal('', "El Texto 3 es muy extenso", 'error');
                }
                else {
                    cartaServicio.actualizarCartaDatosIniciales(parametros).then(function (response) {
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {

                            if (response.data.Exito) {
                                guardarCapituloModificado();
                                //vm.plantillaCarta = {};
                                //vm.carta = {};
                                vm.listaCampos = [];

                                vm.ActivarEditarSinCarga();
                                obtenerPlantillaCarta();
                                
                                parent.postMessage("cerrarModal", window.location.origin);
                                utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                                

                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }

                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                }
            }
            
        }

        function consultarAutorizacion() {
            cartaServicio.ObtenerProyectosCartaTramite(vm.tramiteId).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    var datosRadicado = false;
                    var autorizacion = response.data;
                    if (!datosRadicado && (autorizacion != null || autorizacion != undefined)) {
                        vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY")
                        vm.numeroRadicado = autorizacion.NumeroRadicacion == '' ? '0' : autorizacion.NumeroRadicacion;
                        datosRadicado = true;
                    }

                    if (vm.numeroRadicado == undefined || vm.numeroRadicado === 0) {
                        cartaServicio.ConsultarCartaConcepto(vm.tramiteId).then(function (response) {
                            if (response.statusText === "OK" || response.status === 200) {
                                {
                                    vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY");
                                    vm.numeroRadicado = response.data.RadicadoEntrada == '' ? '0' : response.data.RadicadoEntrada;

                                }
                            }
                        });

                    vm.datosProyectoAutorizacion.entidadId = autorizacion.CodigoEntidad;
                    vm.datosProyectoAutorizacion.entidad = autorizacion.Entidad;
                    vm.datosProyectoAutorizacion.nombreProyecto = autorizacion.NombreProyecto;
                    vm.datosProyectoAutorizacion.programaCodigo = autorizacion.CodigoPrograma;
                    vm.datosProyectoAutorizacion.programa = autorizacion.Programa;
                    vm.datosProyectoAutorizacion.subProgramaCodigo = autorizacion.CodigoSubprograma;                    
                    vm.datosProyectoAutorizacion.subPrograma = autorizacion.Subprogramal;
                    var n = autorizacion.ConsecutivoCodigoPresupuestal;
                    var codigoPresupuestal = ('0000' + n).slice(-4);
                    //vm.datosProyectoAutorizacion.ProyectoId = codigoPresupuestal;
                    vm.datosProyectoAutorizacion.codigoPresupuestal = codigoPresupuestal;

                    // se consulta la modificacion de leyenda
                    cartaServicio.ObtenerDetalleCartaAL(vm.tramiteId).then(function (response) {
                        if (response === undefined || typeof response === 'string') {
                            vm.mensajeError = response;
                            utilidades.mensajeError(response);
                        } else {
                            var autorizacion = response.data;
                            vm.datosProyectoAutorizacion.NombreActual = autorizacion.NombreActual;
                            vm.datosProyectoAutorizacion.Aclaracion = autorizacion.Aclaracion;
                        }
                    }, error => {
                        console.log(error);
                    });
                }
            }
            });
        }

        function obtenerFechaSinHoras(fecha) {
            let day = fecha.getDate()
            let month = fecha.getMonth() + 1
            let year = fecha.getFullYear()
            vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY")
        }


        vm.getTableHeightCDPGR = function () {
            var rowHeight = 30;
            var headerHeight = 50;
            return {
                height: (((vm.datosCDPGR.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };
        }

        vm.getProgramaHeight = function () {
            var longitud = 1;//vm.proyectosValoresContraCredito.length;

            var height = vm.heigprograma + (longitud * 41);
            return {
                height: height + "px"
            }
        }

        vm.ActivarEditar = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
                //panel.classList.replace("btnguardarDisabledDNP", "btnguardarDNP");
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
                //panel.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                obtenerDatosIniciales();
            }
        }

        vm.ActivarEditarSinCarga = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
            }
        }

        vm.getEstiloBtnGuardar = function () {
            if (vm.disabled == false)
                return "btnguardarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
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


        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-conceptoselaborartecnico');
            vm.seccionCapitulo = span.textContent;


        }

    }

    angular.module('backbone').component('cuerpoAl', {
        templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/componentes/conceptos/elaborarConcepto/Carta/cuerpoAl.html",
        controller: cuerpoAl,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
        }
    });


})();