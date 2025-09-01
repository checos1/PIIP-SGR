(function () {
    'use strict';

    datosDespedidaController.$inject = [
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
        '$location'
    ];



    function datosDespedidaController(
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
        $location
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.nombreSccion = 'Despedida';
        vm.tipoTramite = 1;
        vm.tipoTramite = $sessionStorage.TipoTramiteId;
        vm.tramiteId = $sessionStorage.TramiteId;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.NombreUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.bloquear = true;
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
                            CopiarEntidadId:0,
                            CopiarServidorId:0,
                            CopiarEntidad: '',
                            CopiarServidor: '',
                            Bloquear:0
                        }
                    ]
                }
            ]
        }];

        vm.btnaccordiondatosdespedida = function () {
            vm.abrirAccordion = !vm.abrirAccordion ? true : false;
        }

        function init() {
            obtenerPlantillaCarta();

        }
        vm.rotated = false;
        vm.abrirpaneldespedida = function () {

            var acc = document.getElementById("divdatosdespedida");
            var i;
            var rotated = false;

            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgdatosdespedida'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;
        }
        function guardarConceptoDatosDespedida() {

            vm.Count = 0;
            vm.listaCampos = [];
            vm.listaCamposDetalle = [];

            vm.CartaDatosDespedida.Tramite[0].Copiar.forEach(copiar => {
                vm.Count = vm.Count + 1;
                vm.listaCamposDetalle.push({
                    Id: 0,
                    PlantillaCartaCampoId: 19,
                    DatoValor: copiar.CopiarEntidad,
                    NombreCampo: 'CopiarEntidad',
                    //OrdenAgrupa: vm.Count,
                    OrdenAgrupa: 1,
                })

                vm.listaCamposDetalle.push({
                    Id: 0,
                    PlantillaCartaCampoId: 20,
                    DatoValor: copiar.CopiarServidor,
                    NombreCampo: 'CopiarServidor',
                    /*OrdenAgrupa: vm.Count,*/
                    OrdenAgrupa: 2,
                })
            });

            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 31,
                DatoValor: '',
                NombreCampo: 'Copiar',
                CamposCopiar: vm.listaCamposDetalle
            });
            vm.listaCamposVacio = [];
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 11,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].PalabraFraseDespedida,
                NombreCampo: 'PalabraFraseDespedida',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 16,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].RemitenteDNP,
                NombreCampo: 'RemitenteDNP',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 44,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].Cartafirmada,
                NombreCampo: 'CartaFirmada',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 45,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].UsuarioRemitenteDNP,
                NombreCampo: 'UsuarioRemitenteDNP',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 21,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].PreparoEntidad,
                NombreCampo: 'PreparoEntidad',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 22,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].PreparoColaborador,
                NombreCampo: 'PreparoColaborador',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 25,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].RevisoEntidad,
                NombreCampo: 'RevisoEntidad',
                CamposCopiar: vm.listaCamposVacio
            });
            vm.listaCampos.push({
                Id: 0,
                PlantillaCartaCampoId: 29,
                DatoValor: vm.CartaDatosDespedida.Tramite[0].RevisoColaborador,
                NombreCampo: 'RevisoColaborador',
                CamposCopiar: vm.listaCamposVacio
            });

            vm.CartaDatosDespedida.CartaSeccionId = 0;/*Se obtiene en el procedimiento*/
            vm.CartaDatosDespedida.TipoTramite = 0;
            vm.CartaDatosDespedida.PantillaSeccionId = 3;

            vm.CartaDatosDespedida.Campos = vm.listaCampos;

            var parametros = vm.CartaDatosDespedida;

            cartaServicio.actualizarCartaConceptoDatosDespedida(parametros)
                .then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
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
                        obtenerCartaConceptoDatosdespedida();
                    }
                    else {
                        vm.plantillaCarta = {};
                    }
                });
        }

        function obtenerCartaConceptoDatosdespedida() {

            cartaServicio.obtenerCartaConceptoDatosDespedida(vm.tramiteId,3)
                .then(resultado => {
                    if (resultado != undefined && resultado.data.length > 0) {
                        vm.CartaDatosDespedida = jQuery.parseJSON(resultado.data);
                        $sessionStorage.carta = vm.carta;
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
         
        };
    }

    angular.module('backbone').component('datosDespedida', {

        templateUrl: "src/app/formulario/ventanas/tramites/componentes/carta/datosDespedida.html",
        controller: datosDespedidaController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&'
        }
    });


})();