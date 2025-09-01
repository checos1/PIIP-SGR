(function () {
    'use strict';

    datosDespedidaFormulario.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage',
        'cartaServicio',
        'constantesBackbone',
        '$routeParams',
        'servicioResumenDeProyectos',
        'uiGridConstants',
        '$timeout',
        '$location',
        'justificacionCambiosServicio',
        '$interval',
    ];



    function datosDespedidaFormulario(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage,
        cartaServicio,
        constantesBackbone,
        $routeParams,
        servicioResumenDeProyectos,
        uiGridConstants,
        $timeout,
        $location,
        justificacionCambiosServicio,
        $interval
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        
        vm.nombreSccion = 'Despedida' ;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.NombreUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.bloquear = true;

        vm.disabledc = true;
        vm.Editar = 'EDITAR';
        /* metodos*/

        vm.obtenerPlantillaCarta = obtenerPlantillaCarta;
        vm.guardarDatosUsuario = guardarDatosUsuario;
        vm.obtenerCartaConceptoDatosdespedida = obtenerCartaConceptoDatosdespedida;
        vm.guardarConceptoDatosDespedida = guardarConceptoDatosDespedida;

        /*variables objetos*/
        vm.listaTipoUsuario = [
            { id: '1', nombre: 'Usuario no registrado' },
            { id: '2', nombre: 'Usuario registrado' }];
        vm.listaUsuariosDestinatario = [];
        vm.listaUsuariosDestinatariotmp = [];
        vm.plantillaCarta = {};
        vm.carta = {};
        vm.listaCamposPlantilla = [];
        vm.tipoUsuarioRegistradoMostrar = false;
        vm.tipoUsuarioNoRegistradoMostrar = false;
        vm.abrirAccordion = false;

        //vm.CartaDatosDespedida = [];
        vm.CartaDatosDespedida = [{
            TramiteId: 0,
            CartaId: 0,
            CartaSeccionId: 0,
            TipoTramite: 0,
            PantillaSeccionId: 3,
            Campos: [{
                Id: 0,
                PlantillaCartaCampoId: 0,
                DatoValor: "",
                NombreCampo: "",
                CamposCopiar: [{
                    Id: 0,
                    PlantillaCartaCampoId: 0,
                    DatoValor: "",
                    NombreCampo: "",
                    OrdenAgrupa: 0,
                }],
            }],
            Tramite: [
                {
                    TramiteId: 0,
                    CartaConceptoId: 0,
                    NumeroTramite: '',
                    EntidadId: 0,
                    Entidad: '',
                    Cartafirmada: '',
                    PalabraFraseDespedida: '',
                    RemitenteDNP: '',
                    UsuarioRemitenteDNP: '',
                    PreparoEntidad: '',
                    PreparoColaborador: '',
                    RevisoEntidad: '',
                    RevisoColaborador: '',
                    Copiar: [
                        {
                            CopiarEntidadId: 0,
                            CopiarServidorId: 0,
                            CopiarEntidad: '',
                            CopiarServidor: '',
                            Bloquear: 0
                        }
                    ]
                }
            ]
        }];

        vm.btnaccordiondatosdespedida = function () {
            vm.abrirAccordion = !vm.abrirAccordion ? true : false;
        }

        $scope.$watch('vm.rolanalista', function () {
            if (vm.novalidarconcepto) {
                vm.habilitaBotones = true;
            }
            else {
                if (vm.rolanalista !== '' && vm.rolanalista !== undefined && vm.rolanalista !== null) {
                    vm.habilitaBotones = vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false;
                }
            }
        });

        $scope.$watch('vm.deshabilitar', function () {
            if (vm.novalidarconcepto) {
                vm.habilitaBotones = true;
            }
            else {
                if (vm.deshabilitar === "true") {
                    vm.habilitaBotones = false && vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false; false;
                }
                else if (vm.deshabilitar === "false") {
                    vm.habilitaBotones = true && vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false; false;
                }
            }
        });

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            }
        });

        function init() {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                    vm.tramiteId = vm.tramiteid;
                    vm.tipoTramite = vm.tipotramiteid;
                    vm.abrirpanel();
                    obtenerPlantillaCarta();
                }
            });
            

        }
        vm.rotated = false;

        vm.abrirpanel = function () {
            var panel = document.getElementById('paneldatosdespedida');;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = "710px";
            }
        }

        function guardarConceptoDatosDespedida() {

            vm.Count = 0;
            vm.listaCampos = [];
            vm.listaCamposDetalle = [];

            vm.CartaDatosDespedida.Tramite[0].Copiar.forEach(copiar => {
                vm.Count = vm.Count + 1;
                vm.listaCamposDetalle.push({
                    Id: 0,
                    PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'CopiarEntidad')[0].Id,// 74,
                    DatoValor: copiar.CopiarEntidad,
                    NombreCampo: 'CopiarEntidad',
                    //OrdenAgrupa: vm.Count,
                    OrdenAgrupa: 1,
                })

                vm.listaCamposDetalle.push({
                    Id: 0,
                    PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'CopiarServidor')[0].Id, //75,
                    DatoValor: copiar.CopiarServidor,
                    NombreCampo: 'CopiarServidor',
                    /*OrdenAgrupa: vm.Count,*/
                    OrdenAgrupa: 2,
                })
            });

            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'Copiar')[0].Id,//80,
                DatoValor: '',
                NombreCampo: 'Copiar',
                CamposCopiar: vm.listaCamposDetalle
            });
            vm.listaCamposVacio = [];
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'PalabraFraseDespedida')[0].Id,// 72,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].PalabraFraseDespedida,
                NombreCampo: 'PalabraFraseDespedida',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'RemitenteDNP')[0].Id,// 73,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].RemitenteDNP,
                NombreCampo: 'RemitenteDNP',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'CartaFirmada')[0].Id,// , 81,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].Cartafirmada,
                NombreCampo: 'CartaFirmada',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'UsuarioRemitenteDNP')[0].Id,// 82,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].UsuarioRemitenteDNP,
                NombreCampo: 'UsuarioRemitenteDNP',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'PreparoEntidad')[0].Id,// 76,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].PreparoEntidad,
                NombreCampo: 'PreparoEntidad',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'PreparoColaborador')[0].Id,// 77,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].PreparoColaborador,
                NombreCampo: 'PreparoColaborador',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'RevisoEntidad')[0].Id,//  78,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].RevisoEntidad,
                NombreCampo: 'RevisoEntidad',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: vm.plantillaCarta.PlantillaSecciones[0].PlantillaSeccionCampos.filter(c => c.NombreCampo === 'RevisoColaborador')[0].Id,//  79,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].RevisoColaborador,
                NombreCampo: 'RevisoColaborador',
                CamposCopiar: vm.listaCamposVacio
            });

            vm.CartaDatosDespedida.CartaSeccionId = 0;/*Se obtiene en el procedimiento*/
            vm.CartaDatosDespedida.TipoTramite = 0;
            vm.CartaDatosDespedida.PantillaSeccionId = vm.plantillaCarta.PlantillaSecciones[0].Id;

            vm.CartaDatosDespedida.Campos = vm.listaCampos;

            var parametros = vm.CartaDatosDespedida;

            cartaServicio.actualizarCartaConceptoDatosDespedida(parametros)
                .then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (response.data.Exito) {
                            vm.ActivarEditar();
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                            guardarCapituloModificado();
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }

                });
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
                                    vm.listaCamposPlantilla.push(campo);

                                });
                            });
                            PlantillaSecciones.push(seccion);
                        }
                        vm.plantillaCarta.Id = resultado.data.Id;
                        vm.plantillaCarta.IipoTramiteId = resultado.data.IipoTramiteId;
                        vm.plantillaCarta.PlantillaSecciones = PlantillaSecciones;
                        obtenerCartaConceptoDatosdespedida(vm.plantillaCarta.PlantillaSecciones[0].Id);
                    }
                    else {
                        vm.plantillaCarta = {};
                    }
                });
        }

        function obtenerCartaConceptoDatosdespedida(idPlantillaSeccion) {
            cartaServicio.obtenerCartaConceptoDatosDespedida(vm.tramiteId, idPlantillaSeccion)
                .then(resultado => {
                    if (resultado != undefined && resultado.data.length > 0) {
                        vm.CartaDatosDespedida = jQuery.parseJSON(resultado.data);
                        $sessionStorage.carta = vm.carta;

                        //ampliar el height
                        var acc = document.getElementById("paneldatosdespedida");
                        var panel = acc;
                        var alturaSegunFilas = vm.CartaDatosDespedida.Tramite[0].Copiar.length * 80;
                        var medida = panel.scrollHeight + alturaSegunFilas;
                        panel.style.maxHeight = medida + "px";
                    }
                })

        }



        function guardarDatosUsuario() {
            var parametros = {
                NombreUsuario: vm.carta.ListaCartaSecciones[0].ListaCartaCampo[3].DatoValor, //nombreUsuarioDestinatario,
                Cargo: vm.carta.ListaCartaSecciones[0].ListaCartaCampos[4].DatoValor, //cargoUsuarioDestinatario,
                Email: vm.carta.ListaCartaSecciones[0].ListaCartaCampos[5].DatoValor //correoUsuarioDestinatario,
            };
            cartaServicio.ActualizarUsuarioDestinatario(parametros, vm.tramiteId, vm.ca);
        }

        vm.ActivarEditar = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabledc == true) {
                vm.Editar = "CANCELAR";
                vm.disabledc = false;
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabledc = true;
                obtenerPlantillaCarta();
            }
        }

        vm.getEstiloBtnGuardar = function () {
            if (vm.disabledc == false)
                return "btnguardarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        vm.Add = function () {
            //Add the new item to the Array.

            var cajasdinamicas = {}
            cajasdinamicas.CopiarEntidad = "";
            cajasdinamicas.CopiarServidor = "";
            vm.CartaDatosDespedida.Tramite[0].Copiar.push(cajasdinamicas);

            var acc = document.getElementById("paneldatosdespedida");
            var panel = acc;

            var medida = panel.scrollHeight + 80;
            panel.style.maxHeight = medida + "px";

        }

        vm.longitud = 0;
        vm.msjsucces = "";
        vm.delete = function (fila) {
            var msj = "La línea de información con copia a  "
                + vm.CartaDatosDespedida.Tramite[0].Copiar[fila].CopiarEntidad + " - "
                + vm.CartaDatosDespedida.Tramite[0].Copiar[fila].CopiarServidor
                + " se borrará. ¿Esta seguro de continuar?";
            vm.longitud = vm.CartaDatosDespedida.Tramite[0].Copiar.length;

            utilidades.mensajeWarning(msj,
                function funcionContinuar() {
                    vm.msjsucces = "se ha borrado la línea de información con copia a  "
                        + vm.CartaDatosDespedida.Tramite[0].Copiar[fila].CopiarEntidad + " - "
                        + vm.CartaDatosDespedida.Tramite[0].Copiar[fila].CopiarServidor;

                    vm.CartaDatosDespedida.Tramite[0].Copiar.splice(fila, 1);

                    var acc = document.getElementById("paneldatosdespedida");
                    var panel = acc;
                    var medida = panel.scrollHeight - 80;
                    panel.style.maxHeight = medida + "px";


                    $timeout(function () {
                        if (vm.CartaDatosDespedida.Tramite != undefined) {
                            if (vm.CartaDatosDespedida.Tramite[0].Copiar.length < vm.longitud) {
                                utilidades.mensajeSuccess(vm.msjsucces, false, false, false, "<h2 style='color:#069169'>Los datos fueron eliminados.</h2>");
                                vm.longitud = vm.CartaDatosDespedida.Tramite[0].Copiar.length;
                            }
                        }
                    }, 300);
                },
                function funcionCancelar() {
                },
                "Aceptar",
                "Cancelar",
                "Los datos serán eliminados."
            );
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
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;


        }

    }

    angular.module('backbone').component('datosDespedidaFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/elaborarConcepto/datosDespedidaFormulario.html",
        controller: datosDespedidaFormulario,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
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
            deshabilitar: '@',
            rolanalista: '@',
            novalidarconcepto: '@'
        }
    });


})();