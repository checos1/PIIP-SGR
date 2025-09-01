(function () {
    'use strict';

    modalVerCargaDatosController.$inject = [
        'objCarga',
        '$uibModalInstance',
        'servicioCargaDatos',
        'utilidades',
    ];

    function modalVerCargaDatosController(
        objCarga,
        $uibModalInstance,
        servicioCargaDatos,
        utilidades,
    ) {
        var vm = this;
        vm.carga = objCarga;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.listaDynamic = [];
        vm.gridOptions;
        vm.columnDefs = [];

        //vm.opciones = {
        //    cambiarTipoEntidadFiltro: cambiarTipoEntidadFiltro,
        //    nivelJerarquico: 1,
        //    gridOptions: {
        //        //showHeader: true,
        //        paginationPageSizes: [5, 10, 15, 25, 50, 100],
        //        paginationPageSize: 10,
        //        expandableRowHeight: 220
        //    }
        //};

        /// Comienzo
        vm.$onInit = function () {
            servicioCargaDatos.obtenerDatosMongoDb(vm.carga.idArchivo)
                .then(function (response) {
                    if (response.data) {
                        vm.listaDynamic = response.data.InfoArchivo
                    }
                    var listaTeste = [];
                    //var values = { name: 'misko', gender: 'male' };
                    var log = [];
                    angular.forEach(vm.listaDynamic, function (value, key) {
                        //if (value != '_t: DynamicType') {

                        //    listaTeste.push(value)
                        //}
                        delete value._t;
                    //    console.log(value);
                        //this.push(key + ': ' + value);
                    }, log);

                    expect(log).toEqual(['_t: DynamicType']);

                    //Object.keys(vm.listaDynamic).forEach(function (key) {
                    //    vm.columnDefs.push({
                    //        field: key,
                    //        displayName: key,
                    //        enableHiding: false
                    //    })


                    //    console.table('Key : ' + key + ', Value : ' + data[key])
                    //})


                    //vm.gridOptions = {
                    //    columnDefs: vm.columnDefs,
                    //    enableVerticalScrollbar: true,
                    //    enableSorting: true,
                    //    showHeader: true,
                    //    showGridFooter: false
                    //};
                    //vm.gridOptions = vm.listaDynamic;

                    //vm.listaDynamic.forEach(item => {

                    //    usuario.idEntidad = item.IdEntidad;
                    //            usuario.nombreEntidad = item.agrupadorEntidad + ' - ' + item.entidad;
                    //            usuario.IdEntidad = item.IdEntidad;
                    //        });



                 //   console.log(response.data)
                });
        }



    }

    angular.module('backbone.entidades').controller('modalVerCargaDatosController', modalVerCargaDatosController);
})();