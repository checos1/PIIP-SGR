(function () {
    'use strict';
    angular.module('backbone')
        .controller('addCofinanciacionSgrController', addCofinanciacionSgrController);

    addCofinanciacionSgrController.$inject = [
        '$scope',
        '$uibModalInstance',
        'previosSgrServicio',
        'utilidades',
        'vigenciaFuentesNoSgr',
        'valorSolicitadoFuentesNoSgr',
        'bpin',
        'vigencia'
    ];

    function addCofinanciacionSgrController(
        $scope,
        $uibModalInstance,
        previosSgrServicio,
        utilidades,
        vigenciaFuentesNoSgr,
        valorSolicitadoFuentesNoSgr,
        bpin,
        vigencia
    ) {
        const vm = this;
        vm.init = init;
        vm.MostarTabla = "1";
        vm.validacion = "";
        vm.cerrar = cerrar;
        vm.guardar = guardar;
        vm.valorSolicitadoNoSGR = valorSolicitadoFuentesNoSgr;
        vm.valorTotalSolicitadoNoSGRCofinanciado = 0;
        vm.lstCofinanciadores = {
                ProyectoId: bpin,
                Vigencia: vigencia,
                VigenciasFuentes: [],
        }
        vm.datosSinGuardar = false;

        $scope.$watch('vm.lstCofinanciadores.VigenciasFuentes', function () {
            if (vm.lstCofinanciadores.VigenciasFuentes != undefined) {
                CalcularTotal();
            }
        }, true);

        function CalcularTotal() {
            vm.valorTotalSolicitadoNoSGRCofinanciado = 0;

            if (vm.lstCofinanciadores.VigenciasFuentes) {
                vm.lstCofinanciadores.VigenciasFuentes.forEach(VigenciaFuente => {
                    if (VigenciaFuente) {                        
                        vm.valorTotalSolicitadoNoSGRCofinanciado += parseFloat(VigenciaFuente.Valor === undefined ? 0 : VigenciaFuente.Valor);                         
                    }
                });
            }
        }

        // Comienzo
        vm.init = function () {
            vm.IsValid = true;
            vm.obtenerTipoCofinanciador();
            vm.CargarCofinanciadores();
        };

        function init() {
            vm.IsValid = true;
            vm.obtenerTipoCofinanciador();
            vm.CargarCofinanciadores();
        }

        function cerrar() {
            if (vm.datosSinGuardar == true) {

                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    $uibModalInstance.close();

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
            else {
                $uibModalInstance.close();
            }
        }

        vm.obtenerTipoCofinanciador = function () {

            var listaTipoCofinanciador = [];

            return previosSgrServicio.ObtenerTiposCofinanciaciones()
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    
                    console.log(respuesta);
                    var arregloTipoCofinanciador = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arregloTipoCofinanciador.length; ls++) {
                        if (arregloTipoCofinanciador[ls].Name != "Privado") {
                            var tipoC = {
                                "Name": arregloTipoCofinanciador[ls].Name,
                                "Id": arregloTipoCofinanciador[ls].Id,
                            }
                            listaTipoCofinanciador.push(tipoC);
                        }
                    }
                    vm.listaTipoCofinanciador = listaTipoCofinanciador;

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar la lista TipoCofinanciador");
                });
        };

        vm.CargarCofinanciadores = function () {
            
            return previosSgrServicio.consultarDatosAdicionalesCofinanciadorNoSGR(bpin, vigencia, vigenciaFuentesNoSgr)
                .then(respuesta => {
                    if (respuesta.data) {
                        vm.lstCofinanciadores = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));                        
                    }

                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la lista de proyectos");
                });
        };

        vm.EliminarDatosCofinanciador = function (fuenteIndex) {
            vm.lstCofinanciadores.VigenciasFuentes.splice(fuenteIndex, 1);
            vm.datosSinGuardar = true;
        };

        vm.TipoCofinanciadorChanged = function () {

            limpiarFormulario(false);

            if (vm.idtipoCofinanciador != "") {
                var lblCodigo = document.getElementById('lblCodigo');
                var txtNombre = document.getElementById('txtNombre');
                var btnBusqueda = document.getElementById('btnBusqueda');

                if (vm.idtipoCofinanciador == 1) {
                    lblCodigo.textContent = "BPIN";
                    txtNombre.classList.add('disabled');
                    btnBusqueda.classList.remove('hidden');
                }
                else {
                    lblCodigo.textContent = "Código";
                    txtNombre.classList.remove('disabled');
                    btnBusqueda.classList.add('hidden');
                }
            }
        };

        vm.adicionarDatosCofinanciador = function () {
            validar();
            if (!vm.isValid) return;

            let nombreFuente = $("#selTipoCofinanciador option:selected").html().replace(/^\s+|\s+$/gm, '');
            if (vm.lstCofinanciadores.VigenciasFuentes == 'undefined' || vm.lstCofinanciadores.VigenciasFuentes == null) {
                vm.lstCofinanciadores.VigenciasFuentes = [];
            }
            var txtNombre = document.getElementById('txtNombre').value;
            let valorCofinanciadorM = vm.valorCofinanciador === 0 ? 0 : typeof vm.valorCofinanciador == "string" ? parseFloat(vm.valorCofinanciador.replace(",", ".")) : vm.valorCofinanciador;
            vm.lstCofinanciadores.VigenciasFuentes.push({ VigenciaFuente: vigenciaFuentesNoSgr, Fuente: nombreFuente, TipoCofinanciadorId: vm.idtipoCofinanciador, CodigoCofinanciador: vm.codigoCofinanciador, Nombre: txtNombre, Valor: valorCofinanciadorM }); 

            limpiarFormulario(true);

            vm.datosSinGuardar = true;
        };

        function guardar() {
            if (vm.valorTotalSolicitadoNoSGRCofinanciado != vm.valorSolicitadoNoSGR && vm.valorTotalSolicitadoNoSGRCofinanciado > 0) {
                utilidades.mensajeError("El valor distribuido cofinanciado:$"
                    + ConvertirNumero(vm.valorTotalSolicitadoNoSGRCofinanciado) + " debe ser igual al valor solicitado: $" + ConvertirNumero(vm.valorSolicitadoNoSGR) , false);
                return;
            }
            previosSgrServicio.registrarDatosAdicionalesCofinanciadorFuentesNoSGR(vm.lstCofinanciadores)
                .then(resultado => {
                    if (resultado.data != undefined) {
                        if (resultado.data.Status) {
                            utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                            /*cerrar('ok');*/
                        } else {
                            swal("Error al realizar la operación", resultado.data.Message, 'error');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });

            $uibModalInstance.close();

            vm.datosSinGuardar = false;
        }

        function validar() {
            vm.isValid = true;
            vm.validacion = "<ul>";

            if (vm.idtipoCofinanciador == undefined) {
                vm.validacion = vm.validacion + "<li>Debe seleccionar la fuente de financiación</li>";
                vm.isValid = false;
            }
            if (vm.codigoCofinanciador == undefined) {
                vm.validacion = vm.validacion + "<li>Campo Código/BPIN es obligatorio</li>";
                vm.isValid = false;
            }
            if (document.getElementById('txtNombre').value == "") {
                vm.validacion = vm.validacion + "<li>Campo nombre es obligatorio</li>";
                vm.isValid = false;

            }
            if (vm.valorCofinanciador <= 0 || vm.valorCofinanciador == undefined) {
                vm.validacion = vm.validacion + "<li>Campo valor a financiar es obligatorio</li>";
                vm.isValid = false;
            }

            vm.validacion = vm.validacion + "</ul>";
        }

        function limpiarFormulario(LimpiarTipoCofinanciador) {

            if (LimpiarTipoCofinanciador == true) { vm.idtipoCofinanciador = ""; }
            vm.codigoCofinanciador = "";
            document.getElementById('txtNombre').value = "";
            vm.valorCofinanciador = 0;
        }

        vm.mostrarInconsistencia = function () {
      /*      alert('MostrarInconsistencia');*/
            /*swal("Se han presentado inconsistencias por favor verifique.", "<p>hola</p><p>hola</p>", 'error');*/

            swal({
                title: "Se han presentado inconsistencias por favor verifique.",
                text: vm.validacion,
                type: "error",
                html: true,
                confirmButtonText: "Aceptar",
                closeOnConfirm: true,
            });
        }

        vm.buscarProyecto = function () {
            var listaProyectos = [];
            if (bpin == vm.codigoCofinanciador) {
                utilidades.mensajeError("El Bpin a ingresar no puede ser igual al del proyeco actual.", false);
                return false;
            }
            if (vm.codigoCofinanciador == null || vm.codigoCofinanciador == "") {
                utilidades.mensajeError("Debe ingresar un Bpin para la busqueda de proyectos.", false);
                return false;
            }

            return previosSgrServicio.obtenerProyectos(vm.codigoCofinanciador)
                .then(respuesta => {
                    if (!respuesta.data) {
                        utilidades.mensajeError("Bpin no existe, verifiquelo por favor.", false);
                        return;
                    }

                    console.log(respuesta);
                    var arregloProyectos = jQuery.parseJSON(respuesta.data);
                    var txtNombre = document.getElementById('txtNombre');
                    for (var ls = 0; ls < arregloProyectos.length; ls++) {
                        //var proyectos = {
                        //    "ProyectoNombre": arregloProyectos[ls].ProyectoNombre,
                        //    "ProyectoId": arregloProyectos[ls].ProyectoId,
                        //}
/*                        listaProyectos.push(proyectos);*/
/*                        textNombre.value = arregloProyectos[ls].ProyectoNombre;*/
                        txtNombre.value = arregloProyectos[ls].ProyectoNombre;
/*                        vm.proyectoIdBpin = arregloProyectos[ls].ProyectoId;*/
                    }
             /*       vm.listaProyectos = listaProyectos;*/

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar la lista de proyectos");
                });
        };

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        vm.validarTamanio = function (event) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            event.target.value = event.target.value.replace(",", ".");

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 16;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 19;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
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
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.actualizaFila = function (event) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = event.target.value === 0 ? 0 : typeof event.target.value == "string" ? parseFloat(event.target.value.replace("-", "")) : event.target.value;
            event.target.value = parseFloat(event.target.value.replace(",", "."));
        }
    }
 
   
})();