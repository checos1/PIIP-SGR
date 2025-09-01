(function () {
    'use strict';

    datosInicialesController.$inject = [
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



    function datosInicialesController(
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
        vm.nombreSccion = 'Datos Iniciales';
        vm.tipoTramite = 1;
        vm.tipoTramite = $sessionStorage.TipoTramiteId;
        vm.tramiteId = $sessionStorage.TramiteId;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.entidad = $sessionStorage.usuario.permisos.Entidades[0].NombreEntidad;

        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));

        /* metodos*/
        vm.guardarDatosIniciales = guardarDatosIniciales;
        vm.obtenerDatosIniciales = obtenerDatosIniciales;
        vm.verificaUsuarioDestinatario = verificaUsuarioDestinatario;
        vm.obtenerPlantillaCarta = obtenerPlantillaCarta;
        vm.obtenerUsuariosRegistrados = obtenerUsuariosRegistrados;
        vm.guardarDatosIniciales = guardarDatosIniciales;
        vm.cargarusuariosdestinatarios = cargarusuariosdestinatarios;
        vm.guardarDatosUsuario = guardarDatosUsuario;
        vm.cambiarusuarioregistrado = cambiarusuarioregistrado;

        /*variables objetos*/
        vm.listaTipoUsuario = [
            { id: '1', nombre: 'Usuario no registrado' },
            { id: '2', nombre: 'Usuario registrado' }];
        vm.listaUsuariosDestinatario = [];
        vm.listaUsuariosDestinatariotmp = [];
        vm.plantillaCarta = {};
        vm.carta = {};
        vm.listaCampos = [];
        vm.tipoUsuarioRegistradoMostrar = false;
        vm.tipoUsuarioNoRegistradoMostrar = false;
        vm.abrirAccordion = false;

        vm.btnaccordiondatosiniciales = function () {
            vm.abrirAccordion = !vm.abrirAccordion ? true : false;
        }



        function init() {
            obtenerPlantillaCarta();

        }
        vm.rotated = false;
        vm.abrirpanel = function () {

            var acc = document.getElementById('divdatosiniciales');
            var i;
            var rotated = false;


            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgdatosiniciales'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;
        }



        function guardarDatosIniciales() {
            //ajusta a fecha dd/MM/yyyy
            var fechaI = vm.carta.ListaCartaSecciones[0].ListaCartaCampos[1].DatoValor;
            var mes = (fechaI.getMonth() + 1);
            var dia = (fechaI.getDate());
            var strDate = (dia < 10 ? '0' + dia : dia) + "/" + (mes < 10 ? '0' + mes : mes) + "/" + fechaI.getFullYear();
            vm.carta.ListaCartaSecciones[0].ListaCartaCampos[1].DatoValor = strDate;

            var parametros = vm.carta;
            cartaServicio.actualizarCartaDatosIniciales(parametros).then(function (response) {
                if (response.data && (response.statusText === "OK" || response.status === 200)) {

                    if (response.data.Exito) {
                        obtenerDatosIniciales();
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

        function obtenerUsuariosRegistrados() {
            cartaServicio.obtenerUsuariosRegistrados()
                .then(resultado => {
                    //vm.detalleDatosIniciales = resultado.data;

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
            cartaServicio.obtenerDatosCartaPorSeccion(vm.tramiteId, 1)
                .then(resultado => {
                    if (resultado != undefined && resultado.data != null && resultado.data.length > 0) {
                        var tmp = resultado.data[0];
                        vm.carta.Id = tmp.Id;
                        vm.carta.TramiteId = tmp.TramiteId;
                        vm.carta.Proceso = tmp.Proceso;
                        vm.carta.Entidad = tmp.Entidad;
                        vm.carta.TipoId = vm.tipoTramite;

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
                                        if (iteml.NombreCampo === 'codigoProceso')
                                            campo.DatoValor = vm.numeroTramite;
                                        if (iteml.NombreCampo === 'fecha') {
                                            var fechaArr = campotmp.DatoValor.split('/');
                                            campo.DatoValor = new Date(fechaArr[2] + '-' + fechaArr[1] + '-' + (parseInt(fechaArr[0]) + 1));
                                        }

                                        return campo;
                                    })
                                }

                                cartaSeccion.push(plantillaseccion);

                            });


                        }

                        vm.carta.ListaCartaSecciones = cartaSeccion;
                        cargarusuariosdestinatarios();
                        $sessionStorage.carta = vm.carta;

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

                                if (iteml.NombreCampo === 'codigoProceso')
                                    campo.DatoValor = vm.numeroTramite;
                                if (iteml.NombreCampo === 'fecha') {
                                    var d = new Date();
                                    var mes = (d.getMonth() + 1);
                                    var dia = (d.getDate());
                                    var strDate = (dia < 10 ? '0' + dia : dia) + "/" + (mes < 10 ? '0' + mes : mes) + "/" + d.getFullYear();
                                    campo.DatoValor = strDate;
                                }
                                return campo;
                            })

                        }
                        cartaSeccion.push(plantillaseccion);
                        vm.carta.ListaCartaSecciones = cartaSeccion;
                        cargarusuariosdestinatarios(true);

                    }

                });


        }


        function verificaUsuarioDestinatario() {
            cartaServicio.verificaUsuarioDestinatario()
                .then(resultado => {
                    // vm.detalleDatosIniciales = resultado.data;

                });
        }

        function cargarusuariosdestinatarios(value) {
            if (value) {
                vm.carta.ListaCartaSecciones[0].ListaCartaCampos[3].DatoValor = '';
                vm.carta.ListaCartaSecciones[0].ListaCartaCampos[4].DatoValor = '';
                vm.carta.ListaCartaSecciones[0].ListaCartaCampos[5].DatoValor = '';
            }
            if (vm.carta.ListaCartaSecciones[0].ListaCartaCampos[2].DatoValor == '') {
                vm.tipoUsuarioRegistradoMostrar = false;
                vm.tipoUsuarioNoRegistradoMostrar = false;
            }
            else if (vm.carta.ListaCartaSecciones[0].ListaCartaCampos[2].DatoValor == 1) // tipousuarioid
            {
                vm.tipoUsuarioRegistradoMostrar = false;
                vm.tipoUsuarioNoRegistradoMostrar = true;
                vm.carta.ListaCartaSecciones[0].ListaCartaCampos[6].DatoValor = '';

            }
            else {
                var items = [];
                vm.tipoUsuarioRegistradoMostrar = true;
                vm.tipoUsuarioNoRegistradoMostrar = false;

                cartaServicio.obtenerUsuariosRegistrados(vm.tramiteId, vm.numeroTramite)
                    .then(resultado => {
                        if (resultado != undefined && resultado.data) {
                            vm.listaUsuariosDestinatariotmp = resultado.data;
                            resultado.data.map(function (item, index) {
                                items.push({ id: item.IDUsuarioDNP, valor: item.NombreUsuario + ' - ' + item.Cargo });
                            });
                        }
                        vm.listaUsuariosDestinatario = items;
                    });

            }

        }

        function cambiarusuarioregistrado() {
            vm.tipoUsuarioRegistradoMostrar = true;
            //vm.tipoUsuarioNoRegistradoMostrar = true;
            var usuario = vm.listaUsuariosDestinatariotmp.find(x => x.IDUsuarioDNP === vm.carta.ListaCartaSecciones[0].ListaCartaCampos[6].DatoValor);
            vm.carta.ListaCartaSecciones[0].ListaCartaCampos[3].DatoValor = usuario.NombreUsuario;
            vm.carta.ListaCartaSecciones[0].ListaCartaCampos[4].DatoValor = usuario.Cargo;
            vm.carta.ListaCartaSecciones[0].ListaCartaCampos[5].DatoValor = usuario.Email !== undefined ? usuario.Email : '';
        }


        function guardarDatosUsuario() {


            var parametros = {
                NombreUsuario: vm.carta.ListaCartaSecciones[0].ListaCartaCampo[3].DatoValor, //nombreUsuarioDestinatario,
                Cargo: vm.carta.ListaCartaSecciones[0].ListaCartaCampos[4].DatoValor, //cargoUsuarioDestinatario,
                Email: vm.carta.ListaCartaSecciones[0].ListaCartaCampos[5].DatoValor //correoUsuarioDestinatario,
            };
            cartaServicio.ActualizarUsuarioDestinatario(parametros, vm.tramiteId, vm.ca);
        }


        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }
        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }



    }

    angular.module('backbone').component('datosIniciales', {

        templateUrl: "src/app/formulario/ventanas/tramites/componentes/carta/datosIniciales.html",
        controller: datosInicialesController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&'
        }
    });


})();