(function () {
    'use strict';

    informacionPresupuestalAproFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'constantesBackbone',
        '$timeout',
        '$q',
        'justificacionCambiosServicio',
        'tramiteSGPServicio'
    ];

    function informacionPresupuestalAproFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        constantesBackbone,
        $timeout,
        $q,
        justificacionCambiosServicio,
        tramiteSGPServicio
    ) {
        var vm = this;
        vm.nombreComponente = "informacionpresupuestalfuentes";
        vm.lang = "es";
        vm.IdNivel = $sessionStorage.idNivel;
        vm.disabled = true;
        vm.Editar = 'EDITAR';
        vm.camposActivos = 0;
        vm.dataTemporal = [];
        vm.habilitaBotones = true;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        //variables locales

        vm.ConvertirNumero = ConvertirNumero;
        vm.actualizarFuente = actualizarFuente;
        vm.abrirTooltip = abrirTooltip;
        vm.abrirpanel = abrirpanel;
        vm.listaProyectos = [];
        vm.error = {};

        ////#region Metodos

        $scope.$watch('vm.rolanalista', function () {
            if (vm.rolanalista !== '' && vm.rolanalista !== undefined && vm.rolanalista !== null) {
                vm.habilitaBotones = vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false;
            }
        });

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                consultarFuentes();
            }
        });

      
        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                //ObtenerSeccionCapitulo();
            }
        });

        $scope.$watch('vm.deshabilitar', function () {
            if (vm.deshabilitar === "true") {
                vm.habilitaBotones = false && vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false; false;
            }
            else if (vm.deshabilitar === "false") {
                vm.habilitaBotones = true && vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false; false;
            }

        });

        

      
        function consultarFuentes() {
            vm.listaProyectos = [];
            tramiteSGPServicio.obtenerListaProyectosFuentesAprobada(vm.tramiteid).then(function (rta) {
                if (rta.data !== null) {
                    vm.dataTemporal = rta.data;
                    maperaConsulta();
                }
            });
        }

        function maperaConsulta() {
            vm.listaProyectos = [];
            vm.dataTemporal.map(function (item, index) {
                var fuenteIndexAgregar = 0;
                var proyectoIndex = vm.listaProyectos.findIndex(x => x.ProyectoId === item.ProyectoId);
                if (proyectoIndex < 0) {
                    var Proyecto = {};
                    Proyecto.BPIN = item.BPIN;
                    Proyecto.NombreProyecto = item.NombreProyecto;
                    Proyecto.NombreCorto = mapearNombreProyecto(item.NombreProyecto);
                    Proyecto.ValorTotalNacion = ConvertirNumero(item.ValorTotalNacion === null ? 0 : item.ValorTotalNacion);
                    Proyecto.ValorTotalPropios = ConvertirNumero(item.ValorTotalPropios === null ? 0 : item.ValorTotalPropios);
                    Proyecto.Operacion = item.Operacion;
                    Proyecto.TramiteProyectoId = item.TramiteProyectoId;
                    Proyecto.ProyectoId = item.ProyectoId;
                    Proyecto.TramiteId = vm.tramiteid;
                    Proyecto.totalCSFNacion = 0;
                    Proyecto.totalSSFNacion = 0;
                    Proyecto.totalCSFPropios = 0;
                    Proyecto.totalSSFPropios = 0;
                    Proyecto.totalPropios = 0;
                    Proyecto.totalNacion = 0;
                    Proyecto.ValorTotalInicialProyecto = 0;
                    Proyecto.ValorTotalVigenteProyecto = 0;
                    Proyecto.ValorTotalSolicitadoProyecto = 0;
                    Proyecto.ValorTotalAprobadoProyecto = 0;
                    vm.listaProyectos.push(Proyecto);
                    proyectoIndex = vm.listaProyectos.findIndex(x => x.ProyectoId === item.ProyectoId);
                    vm.listaProyectos[proyectoIndex].ListaFuentes = [];
                }
                item.ListaFuentes.map(function (itemfuente) {
                    var fuenteIndex = vm.listaProyectos[proyectoIndex].ListaFuentes.findIndex(x => x.FuenteId === item.FuenteId);
                    if (fuenteIndex < 0) {
                        var fuente = {};
                        fuente.Id = fuenteIndexAgregar;
                        fuente.FuenteId = itemfuente.FuenteId;
                        fuente.NombreCompleto = itemfuente.NombreCompleto;
                        itemfuente.ValorInicialCSF = itemfuente.ValorIncialCSF === undefined ? 0 : itemfuente.ValorIncialCSF;
                        itemfuente.ValorVigenteCSF = itemfuente.ValorVigenteCSF === undefined ? 0 : itemfuente.ValorVigenteCSF;
                        itemfuente.ValorInicialSSF = itemfuente.ValorIncialSSF === undefined ? 0 : itemfuente.ValorIncialSSF;
                        itemfuente.ValorVigenteSSF = itemfuente.ValorVigenteSSF === undefined ? 0 : itemfuente.ValorVigenteSSF;
                        fuente.ValorInicialCSF = ConvertirNumero(itemfuente.ValorIncialCSF === undefined ? 0 : itemfuente.ValorInicialCSF);
                        fuente.ValorVigenteCSF = ConvertirNumero(itemfuente.ValorVigenteCSF === undefined ? 0 : itemfuente.ValorVigenteCSF);
                        fuente.ValorInicialSSF = ConvertirNumero(itemfuente.ValorIncialSSF === undefined ? 0 : itemfuente.ValorInicialSSF);
                        fuente.ValorVigenteSSF = ConvertirNumero(itemfuente.ValorVigenteSSF === undefined ? 0 : itemfuente.ValorVigenteSSF);
                        fuente.GrupoRecurso = itemfuente.GrupoRecurso;
                        fuente.ValorContracreditoCSF = ConvertirNumero(itemfuente.ValorContracreditoCSF === undefined ? 0 : itemfuente.ValorContracreditoCSF);
                        fuente.ValorContracreditoSSF = ConvertirNumero(itemfuente.ValorContracreditoSSF === undefined ? 0 : itemfuente.ValorContracreditoSSF);
                        fuente.TipoValorAprobadoCSF = itemfuente.TipoValorAprobadoCSF;
                        fuente.TipoValorAprobadoSSF = itemfuente.TipoValorAprobadoSSF;
                        fuente.ValorAprobadoCSF = ConvertirNumero(itemfuente.ValorAprobadoCSF === undefined ? 0 : itemfuente.ValorAprobadoCSF);
                        fuente.ValorAprobadoSSF = ConvertirNumero(itemfuente.ValorAprobadoSSF === undefined ? 0 : itemfuente.ValorAprobadoSSF);

                        vm.listaProyectos[proyectoIndex].ListaFuentes.push(fuente);
                        fuenteIndexAgregar++;
                        /*suma lo soclitado segun origen*/
                        if (fuente.GrupoRecurso === 'N') {
                            vm.listaProyectos[proyectoIndex].totalCSFNacion = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalCSFNacion, fuente.ValorAprobadoCSF));
                            vm.listaProyectos[proyectoIndex].totalNacion = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalNacion, fuente.ValorAprobadoCSF));

                        }
                        else if (fuente.GrupoRecurso === 'P') {
                            vm.listaProyectos[proyectoIndex].totalCSFPropios = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalCSFPropios, fuente.ValorAprobadoCSF));
                            vm.listaProyectos[proyectoIndex].totalPropios = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalPropios, fuente.ValorAprobadoCSF));
                        }

                        /*suma lo soclitado segun origen*/
                        if (fuente.GrupoRecurso === 'N') {
                            vm.listaProyectos[proyectoIndex].totalSSFNacion = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalSSFNacion, fuente.ValorAprobadoSSF));
                            vm.listaProyectos[proyectoIndex].totalNacion = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalNacion, fuente.ValorAprobadoSSF));
                        }
                        else if (fuente.GrupoRecurso === 'P') {
                            vm.listaProyectos[proyectoIndex].totalSSFPropios = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalSSFPropios, fuente.ValorAprobadoSSF));
                            vm.listaProyectos[proyectoIndex].totalPropios = ConvertirNumero(sumar(vm.listaProyectos[proyectoIndex].totalPropios, fuente.ValorAprobadoSSF));
                        }

                        
                        /*sumar totales */
                        var sumatmp = sumar(fuente.ValorInicialCSF, fuente.ValorInicialSSF);
                        Proyecto.ValorTotalInicialProyecto = ConvertirNumero(sumar(Proyecto.ValorTotalInicialProyecto, sumatmp));

                        sumatmp = sumar(fuente.ValorVigenteCSF, fuente.ValorVigenteSSF);
                        Proyecto.ValorTotalVigenteProyecto = ConvertirNumero(sumar(Proyecto.ValorTotalVigenteProyecto, sumatmp));

                        sumatmp = sumar(fuente.ValorContracreditoCSF, fuente.ValorContracreditoSSF);
                        Proyecto.ValorTotalSolicitadoProyecto = ConvertirNumero(sumar(Proyecto.ValorTotalSolicitadoProyecto, sumatmp));

                        sumatmp = sumar(fuente.ValorAprobadoCSF, fuente.ValorAprobadoSSF);
                        Proyecto.ValorTotalAprobadoProyecto = ConvertirNumero(sumar(Proyecto.ValorTotalAprobadoProyecto, sumatmp));
                    }

                });


            });


        }

        function actualizarFuente(proyectoId) {
            var listaFuentesAEnviar = [];
            var hayerror = false;
            vm.error = {};
            var mensajeError = '';
            var sumaTemporalNacion = 0;
            var sumaTemporalPropios = 0;
            let fuentes = [];
            
            var datosAGuardar = vm.listaProyectos.find(x => x.ProyectoId === proyectoId);
            var existe = false;
            datosAGuardar.ListaFuentes.map(function (item) {
                var tramiteFuentePrespuestal = {};
                var tramiteFuentePrespuestalindex = listaFuentesAEnviar.findIndex(x => x.IdFuente === item.FuenteId);
                if (tramiteFuentePrespuestalindex < 0) {
                    tramiteFuentePrespuestal = {
                        IdFuente: item.FuenteId
                        , IdProyectoTramite: datosAGuardar.TramiteProyectoId
                        , IdProyecto: datosAGuardar.ProyectoId
                        , IdTramite: datosAGuardar.TramiteId
                        , Accion: datosAGuardar.Operacion == 'Credito' ? 'D' : 'C'
                        , IdTipoValorContracreditoCSF: item.TipoValorContracreditoCSF
                        , IdTipoValorContracreditoSSF: item.TipoValorContracreditoSSF
                    }
                    tramiteFuentePrespuestal.TipoRecurso = item.GrupoRecurso;
                    if (datosAGuardar.Operacion === 'Contracredito') {
                        var valortmpCSF = 0;
                        var valortmpSSF = 0;
                        var ValorContracreditotmpSSF = 0;
                        var ValorContracreditotmpCSF = 0;

                        if (item.ValorAprobadoCSF.toString().includes('.'))
                            valortmpCSF = item.ValorAprobadoCSF.toString().replaceAll('.', '');
                        else
                            valortmpCSF = item.ValorAprobadoCSF;
                        valortmpCSF = parseFloat(valortmpCSF);

                        if (item.ValorAprobadoSSF.toString().includes('.'))
                            valortmpSSF = item.ValorAprobadoSSF.toString().replaceAll('.', '');
                        else
                            valortmpSSF = item.ValorAprobadoSSF;
                        valortmpSSF = parseFloat(valortmpSSF);

                        if (item.ValorContracreditoCSF.toString().includes('.'))
                            ValorContracreditotmpCSF = item.ValorContracreditoCSF.toString().replaceAll('.', '');
                        else
                            ValorContracreditotmpCSF = item.ValorContracreditoCSF;
                        ValorContracreditotmpCSF = parseFloat(ValorContracreditotmpCSF);

                        if (item.ValorContracreditoSSF.toString().includes('.'))
                            ValorContracreditotmpSSF = item.ValorContracreditoSSF.toString().replaceAll('.', '');
                        else
                            ValorContracreditotmpSSF = item.ValorContracreditoSSF;
                        ValorContracreditotmpSSF = parseFloat(ValorContracreditotmpSSF);

                        if (valortmpCSF > ValorContracreditotmpCSF) {
                            hayerror = true;
                            mensajeError = 'El valor aprobado CSF no puede ser mayor al valor solicitado  CSF ';
                        }

                        if (valortmpSSF > ValorContracreditotmpSSF) {
                            hayerror = true;
                            mensajeError = 'El valor aprobado SSF no puede ser mayor al valor solicitado  SSF ';
                        }
                    }
                    if (!hayerror) {
                        if (item.GrupoRecurso === 'N') {
                            sumaTemporalNacion = ConvertirNumero(sumar(sumaTemporalNacion, sumar(item.ValorAprobadoCSF, item.ValorAprobadoSSF)));
                        }
                        else if (item.GrupoRecurso === 'P') {
                            sumaTemporalPropios = ConvertirNumero(sumar(sumaTemporalPropios, sumar(item.ValorAprobadoCSF, item.ValorAprobadoSSF)));
                        }
                        tramiteFuentePrespuestal.ValorAprobadoCSF = limpiaNumero(item.ValorAprobadoCSF);
                        tramiteFuentePrespuestal.ValorAprobadoSSF = limpiaNumero(item.ValorAprobadoSSF)
                        listaFuentesAEnviar.push(tramiteFuentePrespuestal);

                        if (parseFloat(limpiaNumero(sumaTemporalNacion)) > parseFloat(limpiaNumero(datosAGuardar.ValorTotalNacion))) {
                            hayerror = true;
                            mensajeError = 'Suma del valor aprobado nación de las fuentes en el proyecto no puede ser mayor al valor total nación ';
                        }
                        else if (parseFloat(limpiaNumero(sumaTemporalPropios)) > parseFloat(limpiaNumero(datosAGuardar.ValorTotalPropios))) {
                            hayerror = true;
                            mensajeError = 'Suma del valor aprobado propios de las fuentes en el proyecto no puede ser mayor al valor total propios ';
                        }
                    }


                    if (!hayerror) {
                        let c = {
                            TramiteId: tramiteFuentePrespuestal.IdTramite,
                            ProyectoId: tramiteFuentePrespuestal.IdProyecto,
                            EntidadId: $sessionStorage.EntidadId === undefined ? 0 : $sessionStorage.EntidadId,
                            FuenteId: tramiteFuentePrespuestal.IdFuente,
                            TipoAccion: tramiteFuentePrespuestal.Accion,
                            ValorAprobadoCSF: tramiteFuentePrespuestal.ValorAprobadoCSF,
                            ValorAprobadoSSF: tramiteFuentePrespuestal.ValorAprobadoSSF
                        };
                        fuentes.push(c);
                    }
                }
                
                

                
            });
            if (hayerror) {
                utilidades.mensajeError(
                    "Verifique los campos señalados",
                    null,
                    "Hay datos que presentan inconsistencias.");
                cargarError(proyectoId, mensajeError);
            }
            else {
                tramiteSGPServicio.guardarFuentesTramiteProyectoAprobacion(fuentes)
                    .then(function (response) {
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {

                            if (response.data.Exito) {
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                consultarFuentes();
                                guardarCapituloModificado();
                                vm.ActivarEditar(0);
                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                
            }
        }

     

        vm.initInformacionAproPresupuestal = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        }

        vm.ActivarEditar = function (proyectoId) {
            var guardarActivo = true;
            vm.camposActivos = 0;
            var contracreditoSindiligenciar = false;
            var mensajeError = '';
            var proyecto = vm.listaProyectos.find(x => x.ProyectoId === proyectoId);
            if (proyecto != undefined && proyecto.Operacion === 'Credito') {
                //vm.listaProyectos.map(function (item) {
                if (proyecto.Operacion === 'Contracredito') {
                    if (parseFloat(limpiaNumero(proyecto.totalNacion)) <= 0 || parseFloat(limpiaNumero(proyecto.totalPropios <= 0))) {
                        contracreditoSindiligenciar = true;
                        mensajeError = 'Hay proyectos contracredito sin diligenciar.';
                    }
                }
                //});
            }
            if (contracreditoSindiligenciar) {
                cargarError(proyectoId, mensajeError);
            }
            else {
                var panel = document.getElementById('Guardar' + proyectoId);
                var panelEditar = document.getElementById('Editar' + proyectoId);
                if (panel.className.includes('btnguardarDisabledDNP'))
                    guardarActivo = false;
                /*desactiva controles*/
                vm.listaProyectos.map(function (item) {
                    var paneltmp = document.getElementById('Guardar' + item.ProyectoId);
                    if (panel.className.includes("btnguardarDNP"))
                        paneltmp.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                    var panelEditartemp = document.getElementById('Editar' + item.ProyectoId);
                    panelEditartemp.innerText = "EDITAR";
                });


                if (guardarActivo === false) {
                    vm.camposActivos = proyectoId;
                    panel.classList.replace("btnguardarDisabledDNP", "btnguardarDNP");
                    panel.removeAttribute("disabled");
                    panelEditar.innerText = "CANCELAR";

                }
                else {
                    vm.camposActivos = 0;
                    //panel.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                    //panel.classList.add("disabled");
                    panelEditar.innerText = "EDITAR";
                    maperaConsulta();
                }
            }
        }

        vm.actualizaFilaIP = function (event, fuenteId, proyectoId) {
            event.target.value = parseFloat(event.target.value.replace(".", ""));
            if (event.target.value === '' || event.target.value === 'NaN')
                event.target.value = 0;
            return ConvertirNumero(event.target.value);
            
        }

        vm.mostrarOcultarFuentes = function (objeto) {
            var variable = $("#ico" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#ico" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#ico" + objeto).attr("src", "Img/btnMas.svg");
            }
        }

        function cargarError(proyectoId, mensajeError) {
            vm.error.ProyectoId = proyectoId;
            vm.error.Mensaje = mensajeError;
            vm.error.data = '';
        }

        function mapearNombreProyecto  (nombreProyecto) {
            if (nombreProyecto !== undefined && nombreProyecto.length > 80)
                return nombreProyecto.substring(0, 80);
            else
                return nombreProyecto;

        }

        vm.verNombreCompleto = function (idVerMas, idProyecto) {
            if (document.getElementById(idVerMas).classList.contains("proyecto-nombreFP")) {
                document.getElementById(idVerMas).classList.remove("proyecto-nombreFP");
                document.getElementById(idVerMas).classList.add("proyecto-nombreFP-completo");
                document.getElementById(idVerMas).innerText = vm.listaProyectos.find(w => w.ProyectoId == idProyecto).NombreProyecto;
                document.getElementById("btnVerMasNombre-" + idProyecto).innerText = "Ver menos"
            } else {
                document.getElementById(idVerMas).classList.remove("proyecto-nombreFP-completo");
                document.getElementById(idVerMas).classList.add("proyecto-nombreFP");
                document.getElementById(idVerMas).innerText = vm.listaProyectos.find(w => w.ProyectoId == idProyecto).NombreCorto;
                document.getElementById("btnVerMasNombre-" + idProyecto).innerText = "Ver mas"
            }
        }

        vm.validateFormatIP = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(",");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 16) {
                    event.preventDefault();
                }
            }
        }

        vm.limpiarErrores = function () {
            vm.errores002 = null;
            vm.errorList = new Object();
        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: p.Descripcion,
                        data: p.Data
                    });
                }
            });
        }

        function sumar(value1, value2) {
            if (value1.toString().includes('.'))
                value1 = value1.toString().replaceAll('.', '')
            if (value2.toString().includes('.'))
                value2 = value2.toString().replaceAll('.', '')
            return parseFloat(value1) + parseFloat(value2);
        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 0,
            }).format(numero);
        }

        function abrirTooltip() {
            utilidades.mensajeInformacionV('Esta es la explicación de la carga de archivos... un hecho establecido hace '
                + 'demasiado tiempo que un lector se distraerá con el contenido del texto de '
                + 'un sitio mientras que mira su diseño.El punto de usar Lorem Ipsum es que '
                + 'tiene una distribución más o menos normal de las letras, al contrario de '
                + 'usar textos como por ejemplo "Contenido aquí, contenido aquí".', false, "Carga de archivos")
        }


     


        vm.rotated = false;
        function abrirpanel() {

            var acc = document.getElementById('divcargaarchivo');
            var i;
            var rotated = false;


            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgcargaarchivo'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;

            //ObtenerSeccionCapitulo();
        }

     
     
        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {

            ObtenerSeccionCapitulo();
            var data = {
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
        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
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


        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {

            var campoObligatorioJustificacion = document.getElementById("IPF003-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

   
            var campoObligatorioJustificacion = document.getElementById("IPF004-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            

            if (vm.listaProyectos !== undefined) {
                vm.listaProyectos.map(function (proyecto) {
                    //Busca los proyectos y borra los errores

                    var campoObligatorioJustificacion = document.getElementById("IPF003-" + proyecto.ProyectoId);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                   
                    proyecto.ListaFuentes.map(function (fuente) {
                        
                            var campoObligatorioJustificacion = document.getElementById("IPF004-" + fuente.FuenteId + '-' + proyecto.ProyectoId);
                            if (campoObligatorioJustificacion != undefined) {
                                campoObligatorioJustificacion.innerHTML = "";
                                campoObligatorioJustificacion.classList.add('hidden');
                            }
                        
                    });

                    

                });
            }

           

           

           


        }



        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (erroresJson != undefined) {
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }



        vm.validarIPF003 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF003-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF003Grill = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF003-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF00Grilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF003-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF004Grilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF004-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

       

        vm.validarIPF004 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF004-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

     

        vm.errores = {
            'IPF003-': vm.validarIPF00Grilla,//Este sirve para el error del 3 y del 4 del proyecto para que no se repitan los iconos
            'IPF003': vm.validarIPF003,
            'IPF004-': vm.validarIPF004Grilla,
            'IPF004': vm.validarIPF004,
            
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


    }

    angular.module('backbone').component('informacionPresupuestalAproFormulario', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/comun/informacionPresupuestalAprobacion/informacionPresupuestalAproFormulario.html",
        controller: informacionPresupuestalAproFormulario,
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
            objetonegocioid: '@',
            nombrecomponentepaso: '@',
            rolanalista: '@',
            deshabilitar: '@',
        }
    });

})();
