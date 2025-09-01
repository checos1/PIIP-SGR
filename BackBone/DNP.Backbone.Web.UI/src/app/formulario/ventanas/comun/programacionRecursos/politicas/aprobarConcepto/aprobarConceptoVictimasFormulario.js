(function () {
    'use strict';

    aprobarConceptoVictimas.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'focalizacionAjustesServicio', 'serviciosComponenteNotificaciones', 'trasladosServicio', 'solicitarconceptoServicio', 'comunesServicio', 'archivoServicios'
    ];

    function aprobarConceptoVictimas(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        focalizacionAjustesServicio,
        serviciosComponenteNotificaciones,
        trasladosServicio,
        solicitarconceptoServicio,
        comunesServicio,
        archivoServicios,
    ) {


        var vm = this;
        vm.init = init;
        vm.nombreComponente = "aprobarConceptoVictimasFormulario";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.abrirMensajeInfConceptoRespuesta = abrirMensajeInfConceptoRespuesta;
        vm.coleccion = "tramites";
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.arregloGeneralPTResumen = null;
        vm.verTablaResumen = false;
        vm.TienePoliticas = 0;
        vm.DocumentoJustificacion = "No",
            vm.NombreArchivo = "Justificación modificación política";

        vm.Politicas = [];
        vm.analistas = [];
        vm.PreguntasEnvioPoliticaSubDireccion = [];
        vm.PreguntasEnvioPoliticaSubDireccionOrigen = [];
        vm.EnviosSubdireccion = [];
        vm.cumple = false;
        vm.rolValidadorPolitica = false;
        vm.habilitaBotones = false;// habilita solo en paso 1

        vm.componentesRefresh = [
            'recursosfuentesdefinanc',
            'datosgeneraleslocalizaciones',
            'focalizacionpoliticastransv',
            'focalizacionpolresumendeformul',
            'focalizacionpolpoliticastransv',
        ];

        $scope.$watch('vm.calendariopoliticastransversales', function () {
            if (vm.calendariopoliticastransversales !== undefined && vm.calendariopoliticastransversales !== '')
                vm.habilitaBotones = vm.calendariopoliticastransversales === 'true' ? true : false;
        });

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoid) {
                    vm.CargarInicio();
                }
            }
        });

        function init() {

        }

        vm.CargarInicio = function () {
            setTimeout(function () {
                vm.model = {
                    modulos: {
                        administracion: false,
                        backbone: true
                    }
                }
                vm.ObtenerPreguntasEnvioPoliticaSubDireccion();
                vm.ConsultarArchivosConcepto();
                vm.ValidaRol();
            }, 1000);
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.CargarInicio();
            }
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }

        vm.ValidaRol = function () {
            if ($sessionStorage.usuario.roles.find(x => x.IdRol.toUpperCase() == constantesBackbone.idRControlValidadorPoliticaTransversal)) {
                vm.rolValidadorPolitica = true;
            }
        }

        vm.consultarArchivosTramite = function () {
            var valor = 0;
            var param = {
                bpin: $sessionStorage.idObjetoNegocio,
                idinstancia: $sessionStorage.idInstancia
            };

            archivoServicios.obtenerListadoArchivos(param, "proyectos").then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    if (response != null) {
                        response.forEach(archivo => {
                            if (archivo.status != 'Eliminado') {
                                if (archivo.nombre == vm.NombreArchivo) {
                                    vm.DocumentoJustificacion = "Si";
                                }
                            }
                        });
                    }
                }
            }, error => {
                console.log(error);
            });

        }

        vm.ObtenerPreguntasEnvioPoliticaSubDireccion = function () {

            var PreguntasPoliticasSubdireccion = {
                IdInstancia: $sessionStorage.idInstancia,
                IdProyecto: vm.proyectoid,
                IdUsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                IdNivel: $sessionStorage.idNivel
            };

            return focalizacionAjustesServicio.ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasPoliticasSubdireccion).then(

                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.ResumenSolicitudConcepto = [];
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.PreguntasEnvioPoliticaSubDireccion = jQuery.parseJSON(arreglolistas);
                        if (vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length > 0) {
                            vm.TienePoliticas = vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length;

                            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                                vm.EnviosSubdireccion = [];
                                vm.EnviosSubdireccion = pol.EnviosSubdireccion.filter(x => x.RespuestaEnviada == false && x.Activo == true);

                                pol.VerPolitica = 0;
                                pol.EnviosSubdireccion.forEach(envs => {
                                    envs.VerPregunta = 0;
                                    if (vm.rolValidadorPolitica === true) {
                                        if (envs.IdUsuarioDNP == $sessionStorage.usuario.permisos.IdUsuarioDNP && envs.Activo == true) {
                                            pol.VerPolitica = 1;
                                            envs.VerPregunta = 1;
                                            envs.Preguntas.forEach(pre => {
                                                pre.OpcionesRespuesta = jQuery.parseJSON(pre.OpcionesRespuesta);
                                                pre.Editar = 0;
                                            });
                                        }
                                    }
                                });
                            });

                            vm.PreguntasEnvioPoliticaSubDireccionOrigen = jQuery.parseJSON(arreglolistas);
                        }
                    }
                });
        }

        function abrirMensajeInfConceptoRespuesta() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Política para solicitud de concepto.</span>");
        }

        vm.AbrilNivel = function (politicaId, indexpoliticas) {

            var variable = $("#icojusrespuesta-" + politicaId + "-" + indexpoliticas)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasjusrespuesta-" + politicaId + "-" + indexpoliticas);
            var imgmenos = document.getElementById("imgmenosjusrespuesta-" + politicaId + "-" + indexpoliticas);
            if (variable === "+") {
                $("#icojusrespuesta-" + politicaId + "-" + indexpoliticas).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icojusrespuesta-" + politicaId + "-" + indexpoliticas).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                if (politicaId == pol.PoliticaId) {
                    pol.EnviosSubdireccion.forEach(envs => {
                        if (envs.RespuestaEnviada == true) {
                            envs.Preguntas.forEach(pre => {
                                var radiosirp = document.getElementById("radio" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0);
                                var radionorp = document.getElementById("radio" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1);
                                if (radiosirp != undefined && radionorp != undefined) {
                                    if (pre.Respuesta == "1") {
                                        $('#radio' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0).prop('checked', true);
                                    }
                                    if (pre.Respuesta == "2") {
                                        $('#radio' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1).prop('checked', true);
                                    }
                                }

                            });
                        }
                    });
                }
            });

        }

        vm.ActivarEditar = function (politicaId, resumen) {

            var variable = $("#Editar" + vm.proyectoid)[0].innerText;

            var radiosi = document.getElementById("radio" + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 0);
            var radiono = document.getElementById("radio" + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 1);
            var obs = document.getElementById("obs" + vm.proyectoid + resumen.Preguntas[0].PreguntaId);

            if (variable == "EDITAR") {
                $("#Editar" + vm.proyectoid).html("CANCELAR");

                document.getElementById("Guardarpsca" + vm.proyectoid).classList.remove('btnguardarDisabledDNP');
                document.getElementById("Guardarpsca" + vm.proyectoid).classList.add('btnguardarDNP');
                document.getElementById("Enviarpsca" + vm.proyectoid).classList.remove('btnguardarDNP');
                document.getElementById("Enviarpsca" + vm.proyectoid).classList.add('btnguardarDisabledDNP');
                if (radiosi != undefined) {
                    radiosi.removeAttribute('disabled');
                }
                if (radiono != undefined) {
                    radiono.removeAttribute('disabled');
                }
                if (obs != undefined) {
                    obs.removeAttribute('readonly');
                }
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    setTimeout(function () {
                        utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
                    }, 500);
                    document.getElementById("Guardarpsca" + vm.proyectoid).classList.add('btnguardarDisabledDNP');
                    document.getElementById("Guardarpsca" + vm.proyectoid).classList.remove('btnguardarDNP');
                    var observacionpregunta = "";
                    var respuesta = "";

                    vm.PreguntasEnvioPoliticaSubDireccionOrigen.Politicas.forEach(pol => {
                        if (pol.PoliticaId == politicaId) {
                            pol.EnviosSubdireccion.forEach(envs => {
                                if (envs.EnvioPoliticaSubDireccionIdAgrupa == resumen.EnvioPoliticaSubDireccionIdAgrupa) {
                                    envs.Preguntas.forEach(pre => {
                                        observacionpregunta = pre.ObservacionPregunta;
                                        respuesta = pre.Respuesta;
                                    });
                                }
                            });
                        }
                    });

                    if (radiosi != undefined) {
                        $('#radio' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 0).attr("disabled", true);
                        $('#radio' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 1).prop('checked', false);
                    }

                    if (radiono != undefined) {
                        $('#radio' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 1).attr("disabled", true);
                        $('#radio' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 1).prop('checked', false);
                    }

                    if (obs != undefined) {
                        obs.value = observacionpregunta;
                        $('input[type =obs' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + '], textarea').attr('readonly', 'readonly');
                    }

                    $("#Editar" + vm.proyectoid).html("EDITAR");
                    $("#obserrorconceptoajuste" + vm.proyectoid).attr('disabled', true);
                    $("#obserrorconceptoajuste" + vm.proyectoid).fadeOut();
                    $("#obserrorconceptoajustemsn" + vm.proyectoid).attr('disabled', true);
                    $("#obserrorconceptoajustemsn" + vm.proyectoid).fadeOut();
                    var Errormsn = document.getElementById("obserrorconceptoajustemsn" + vm.proyectoid);
                    if (Errormsn != undefined) {
                        Errormsn.innerHTML = '<span></span>';
                    }

                    $("#radioerrorconceptoajuste" + vm.proyectoid).attr('disabled', true);
                    $("#radioerrorconceptoajuste" + vm.proyectoid).fadeOut();
                    $("#radioerrorconceptoajustemsn" + vm.proyectoid).attr('disabled', true);
                    $("#radioerrorconceptoajustemsn" + vm.proyectoid).fadeOut();
                    var Errormsn = document.getElementById("radioerrorconceptoajustemsn" + vm.proyectoid);
                    if (Errormsn != undefined) {
                        Errormsn.innerHTML = '<span></span>';
                    }


                }, function funcionCancelar(reason) {

                }, null, null, "¿Los posibles datos que haya diligenciado se perderán.");
            }
        }

        vm.GuardarRespuesta = function (politicaId, resumen) {

            validarCampos(politicaId, resumen);
            if (!vm.cumple) {
                utilidades.mensajeError("Hay datos que presentan inconsistencias, Verifique los campos señalados");
                return;
            }

            for (var i = 0; i < vm.EnviosSubdireccion.length; i++) {

                let RespuestaPoliticasSubdireccion = {
                    IdInstancia: $sessionStorage.idInstancia,
                    IdProyecto: vm.proyectoid,
                    IdUsuarioDNP: vm.EnviosSubdireccion[i].IdUsuarioDNP,//$sessionStorage.usuario.permisos.IdUsuarioDNP,
                    IdNivel: $sessionStorage.idNivel,
                    PoliticaId: politicaId,
                    Respuesta: resumen.Preguntas[0].Respuesta,
                    ObservacionPregunta: resumen.Preguntas[0].ObservacionPregunta,
                    EnvioPoliticaSubDireccionIdAgrupa: vm.EnviosSubdireccion[i].EnvioPoliticaSubDireccionIdAgrupa,
                    PreguntaId: resumen.Preguntas[0].PreguntaId
                };


                focalizacionAjustesServicio.GuardarPreguntasEnvioPoliticaSubDireccionAjustes(RespuestaPoliticasSubdireccion).then(function (response) {
                    if (response.statusText === "OK" || response.status === 200) {

                        if (i == vm.EnviosSubdireccion.length) {
                            $("#Editar" + vm.proyectoid).html("EDITAR");
                            document.getElementById("Enviarpsca" + vm.proyectoid).classList.remove('btnguardarDisabledDNP');
                            document.getElementById("Enviarpsca" + vm.proyectoid).classList.add('btnguardarDNP');
                            document.getElementById("Guardarpsca" + vm.proyectoid).classList.remove('btnguardarDNP');
                            document.getElementById("Guardarpsca" + vm.proyectoid).classList.add('btnguardarDisabledDNP');
                            $('#radio' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 0).attr("disabled", true);
                            $('#radio' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + 1).attr("disabled", true);
                            $('input[type =obs' + vm.proyectoid + resumen.Preguntas[0].PreguntaId + '], textarea').attr('readonly', 'readonly');
                            setTimeout(function () {
                                utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                            }, 500);

                            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                                if (pol.PoliticaId == politicaId) {
                                    pol.EnviosSubdireccion.forEach(envs => {
                                        if (envs.EnvioPoliticaSubDireccionIdAgrupa == resumen.EnvioPoliticaSubDireccionIdAgrupa) {
                                            envs.Preguntas.forEach(pre => {
                                                observacionpregunta = resumen.Preguntas[0].ObservacionPregunta;
                                                respuesta = resumen.Preguntas[0].Respuesta;
                                            });
                                        }
                                    });
                                }
                            });
                            vm.PreguntasEnvioPoliticaSubDireccionOrigen.Politicas.forEach(pol => {
                                if (pol.PoliticaId == politicaId) {
                                    pol.EnviosSubdireccion.forEach(envs => {
                                        if (envs.EnvioPoliticaSubDireccionIdAgrupa == resumen.EnvioPoliticaSubDireccionIdAgrupa) {
                                            envs.Preguntas.forEach(pre => {
                                                observacionpregunta = resumen.Preguntas[0].ObservacionPregunta;
                                                respuesta = resumen.Preguntas[0].Respuesta;
                                            });
                                        }
                                    });
                                }
                            });
                        }

                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje);
                    }
                });
            }

        }

        function validarCampos(politicaId, resumen) {
            vm.cumple = false;
            var error = "";
            if (resumen.Preguntas[0].ObservacionPregunta == "" || resumen.Preguntas[0].ObservacionPregunta == null) {
                $("#obserrorconceptoajuste" + vm.proyectoid).attr('disabled', false);
                $("#obserrorconceptoajuste" + vm.proyectoid).fadeIn();
                $("#obserrorconceptoajustemsn" + vm.proyectoid).attr('disabled', false);
                $("#obserrorconceptoajustemsn" + vm.proyectoid).fadeIn();
                var Errormsn = document.getElementById("obserrorconceptoajustemsn" + vm.proyectoid);
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span style="margin: 0 auto;">Campo Obligatorio</span>';
                }
                error = "Observacion obligatoria";
            }
            else if (resumen.Preguntas[0].ObservacionPregunta.length > 5000) {
                $("#obserrorconceptoajuste" + vm.proyectoid).attr('disabled', false);
                $("#obserrorconceptoajuste" + vm.proyectoid).fadeIn();
                $("#obserrorconceptoajustemsn" + vm.proyectoid).attr('disabled', false);
                $("#obserrorconceptoajustemsn" + vm.proyectoid).fadeIn();
                var Errormsn = document.getElementById("obserrorconceptoajustemsn" + vm.proyectoid);
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span style="margin: 0 auto;">Campo supera el limte de 5.000 caracteres</span>';
                }
                error = "Campo supera el limte de 5.000 caracteres";
            }
            else {
                $("#obserrorconceptoajuste" + vm.proyectoid).attr('disabled', true);
                $("#obserrorconceptoajuste" + vm.proyectoid).fadeOut();
                $("#obserrorconceptoajustemsn" + vm.proyectoid).attr('disabled', true);
                $("#obserrorconceptoajustemsn" + vm.proyectoid).fadeOut();
                var Errormsn = document.getElementById("obserrorconceptoajustemsn" + vm.proyectoid);
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span></span>';
                }
            }

            if (resumen.Preguntas[0].Respuesta == "" || resumen.Preguntas[0].Respuesta == null) {
                $("#radioerrorconceptoajuste" + vm.proyectoid).attr('disabled', false);
                $("#radioerrorconceptoajuste" + vm.proyectoid).fadeIn();
                $("#radioerrorconceptoajustemsn" + vm.proyectoid).attr('disabled', false);
                $("#radioerrorconceptoajustemsn" + vm.proyectoid).fadeIn();
                var Errormsn = document.getElementById("radioerrorconceptoajustemsn" + vm.proyectoid);
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span style="margin: 0 auto;">Campo Obligatorio</span>';
                }
                error = "seleccion obligatoria";
            }
            else {
                $("#radioerrorconceptoajuste" + vm.proyectoid).attr('disabled', true);
                $("#radioerrorconceptoajuste" + vm.proyectoid).fadeOut();
                $("#radioerrorconceptoajustemsn" + vm.proyectoid).attr('disabled', true);
                $("#radioerrorconceptoajustemsn" + vm.proyectoid).fadeOut();
                var Errormsn = document.getElementById("radioerrorconceptoajustemsn" + vm.proyectoid);
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span></span>';
                }
            }
            if (error == "") {
                vm.cumple = true;
            }

        }

        vm.Enviar = function (politicaId) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                    for (var i = 0; i < pol.EnviosSubdireccion.length; i++) {


                        let EnvioRespuestaPoliticaSubDireccion = {
                            Id: pol.EnviosSubdireccion[i].EnvioPoliticaSubDireccionIdAgrupa,
                            ProyectoId: vm.PreguntasEnvioPoliticaSubDireccion.ProyectoId,
                            PoliticaId: politicaId,
                            IdUsuarioDNP: pol.EnviosSubdireccion[i].IdUsuarioDNP,

                        };

                        NotificacionUsuarios(politicaId, pol.EnviosSubdireccion[0]);

                        focalizacionAjustesServicio.GuardarRespuestaEnvioPoliticaSubDireccionAjustes(EnvioRespuestaPoliticaSubDireccion).then(function (response) {
                            if (response.statusText !== "OK" || response.status !== 200) {
                                utilidades.mensajeError(response.data.Mensaje);
                            }
                        });

                        if (i + 1 == pol.EnviosSubdireccion.length) {
                            $("#Editar" + vm.proyectoid).html("EDITAR");
                            document.getElementById("Enviarpsca" + vm.proyectoid).classList.remove('btnguardarDNP');
                            document.getElementById("Enviarpsca" + vm.proyectoid).classList.add('btnguardarDisabledDNP');
                            document.getElementById("Guardarpsca" + vm.proyectoid).classList.remove('btnguardarDNP');
                            document.getElementById("Guardarpsca" + vm.proyectoid).classList.add('btnguardarDisabledDNP');
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            vm.CargarInicio();
                            setTimeout(function () {
                                utilidades.mensajeSuccess('', false, false, false, "Su respuesta fue enviada exitosamente al solicitante.");
                            }, 500);
                        }
                    }
                });

            }, function funcionCancelar(reason) {

            }, null, null, "Su respuesta de aprobación de modificación será enviada al solicitante.");

        }

        function NotificacionUsuarios(politicaId, resumen) {
            vm.LstNotificacionFlujo = [];

            var PoliticaTransversal = "";
            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                if (pol.PoliticaId == politicaId) {
                    PoliticaTransversal = pol.NombrePolitica;
                }
            });


            $scope.mydate = new Date();

            var NotificacionFlujo = {
                IdUsuarioDNP: resumen.UsuarioFormulador,
                NombreNotificacion: "Notificar a Dirección Técnica",
                FechaInicio: new Date(),
                FechaFin: new Date(),
                ContenidoNotificacion: "se dio respuesta al concepto tecnico de la politica " + PoliticaTransversal + " y se envía a su bandeja para continuar con el proceso.",
                IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
            }
            vm.LstNotificacionFlujo.push(NotificacionFlujo);

            serviciosComponenteNotificaciones.NotificarUsuarios(vm.LstNotificacionFlujo, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(function (response) {
                if (response.statusText === "OK" || response.status === 200) {

                }
            });

        }

        vm.ConsultarArchivosConcepto = function () {

            let param = {
                proyectoId: vm.proyectoid,
                status: "Nuevo"
            };
            if (!vm.ArchivosAprob) {
                vm.ArchivosAprob = {
                    tieneArchivosAdjuntos: [], idArchivoBlobDescarga: [], nombreArchivoDescarga: [], contentType: [], idMongo: []
                }
            }

            archivoServicios.obtenerListadoArchivos(param, vm.coleccion).then(function (response) {
                vm.ArchivosAprob.tieneArchivosAdjuntos[String(vm.proyectoid)] = false;
                vm.ArchivosAprob.idArchivoBlobDescarga[String(vm.proyectoid)] = "";
                vm.ArchivosAprob.nombreArchivoDescarga[String(vm.proyectoid)] = "";
                vm.ArchivosAprob.contentType[String(vm.proyectoid)] = "";
                vm.ArchivosAprob.idMongo[String(vm.proyectoid)] = "";

                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    if (response.length > 0) {
                        vm.ArchivosAprob.tieneArchivosAdjuntos[String(vm.proyectoid)] = true;
                        vm.ArchivosAprob.idArchivoBlobDescarga[String(vm.proyectoid)] = response[0].metadatos.idarchivoblob;
                        vm.ArchivosAprob.nombreArchivoDescarga[String(vm.proyectoid)] = response[0].nombre;
                        vm.ArchivosAprob.contentType[String(vm.proyectoid)] = response[0].metadatos.contenttype;
                        vm.ArchivosAprob.idMongo[String(vm.proyectoid)] = response[0].id;

                    }
                    else {
                        vm.ArchivosAprob.tieneArchivosAdjuntos[String(vm.proyectoid)] = false;
                        vm.ArchivosAprob.idArchivoBlobDescarga[String(vm.proyectoid)] = "";
                        vm.ArchivosAprob.nombreArchivoDescarga[String(vm.proyectoid)] = "";
                        vm.ArchivosAprob.contentType[String(vm.proyectoid)] = "";
                        vm.ArchivosAprob.idMongo[String(vm.proyectoid)] = "";
                    }
                }
            });
        }

        vm.descargarArchivoBlob = function () {
            archivoServicios.obtenerArchivoBytes(vm.ArchivosAprob.idArchivoBlobDescarga[String(vm.proyectoid)], vm.coleccion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, vm.ArchivosAprob.contentType[String(vm.proyectoid)]);
                const downloadUrl = URL.createObjectURL(blob);
                const a = document.createElement("a");
                a.href = downloadUrl;
                a.download = vm.ArchivosAprob.nombreArchivoDescarga[String(vm.proyectoid)].trimEnd();
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Focalizacion consultaraprobarconceptodt");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.limpiarErrores = function () {
            /*
            var validacionffr1 = document.getElementById(vm.nombreComponente + "-validacionffr1-error");
            var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionffr1-error-mns");
            if (validacionffr1 != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = "";
                    validacionffr1.classList.add('hidden');
                }
            }
            */
        }

        vm.errores = {

        }

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Aprobación concepto política víctimas para la distribución de recursos para el proceso de programación'
                , false, "Detalle política programación")
            // vm.modificodatos = 1;
        }
    }

    angular.module('backbone').component('aprobarConceptoVictimas', {
        templateUrl: "src/app/formulario/ventanas/comun/programacionRecursos/politicas/aprobarConcepto/aprobarConceptoVictimasFormulario.html",
        controller: aprobarConceptoVictimas,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            proyectoid: '@',
            tramiteproyectoid: '@',
            calendariopoliticastransversales: '@'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();