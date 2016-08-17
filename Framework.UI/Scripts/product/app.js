var app = angular.module('myApp', []);
app.controller('productController', ['$scope', '$http', productController]);
app.directive('fileUpload', function () {
    return {
        scope: true,        
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                for (var i = 0; i < files.length; i++) {
                    scope.$emit("fileSelected", { file: files[i] });
                }
            });
        }
    };
});

// Angularjs Controller
function productController($scope, $http) {
    // Declare variable
    $scope.loading = true;
    $scope.updateShow = false;
    $scope.addShow = true;
    var apiGet = "/api/v1/Product/Get/";
    var apiPut = "/api/v1/Product/Put/";
    var apiPost = "/api/v1/Product/Post/";
    var apiDelete = "/api/v1/Product/Delete/";

    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {
            $scope.uploadphoto(args.file);
        });
    });
    // Get All Product
    loadGrid = function () {
        $http.get(apiGet).success(function (data) {
            //debugger;
            $scope.products = data;
        }).error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });
    }

    loadGrid();
    //Insert Product
    $scope.add = function () {
        $scope.loading = true;
        $http.post(apiPost, this.newProduct).success(function (data) {
            loadGrid();
            $scope.updateShow = false;
            $scope.addShow = true;
            $scope.newProduct = '';
        }).error(function (data) {
            $scope.error = "An Error has occured while Adding employee! " + data;
        });
    }

    //Edit Product
    $scope.edit = function () {
        var Id = this.product.Id;
        $http.get(apiGet + Id).success(function (data) {
            $scope.newProduct = data;
            $scope.updateShow = true;
            $scope.addShow = false;
        }).error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });
    }

    $scope.update = function () {
        $scope.loading = true;
        console.log(this.newProduct);
        $http.put(apiPut, this.newProduct).success(function (data) {
            loadGrid();
            $scope.updateShow = false;
            $scope.addShow = true;
            $scope.newProduct.PhotoUrl = '';
            $scope.newProduct = '';
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving employee! " + data;
        });
    }

    //Delete Product
    $scope.delete = function () {
        var Id = this.product.Id;
        $scope.loading = true;
        $http.delete(apiDelete + Id).success(function (data) {
            // $scope.products = data;
            loadGrid();
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving employee! " + data;
        });
    }

    //Cancel Product
    $scope.cancel = function () {
        $scope.updateShow = false;
        $scope.addShow = true;
        $scope.newProduct = '';
    }

    $scope.getImage = function (src) {
        if (src == undefined || src == "") {
            return "/UploadPhotos/NoImage.png";
        } else {
            return src;
        }
    };

    var getPhotoAsFormData = function (data) {
        var photoAsFormData = new FormData();
        photoAsFormData.append("file", data);
        return photoAsFormData;
    }
    $scope.uploadphoto = function (file) {
        if (file == undefined) {
            return
        };
        $http({
            method: 'POST',
            url: "/Api/V1/Product/UploadPhoto",
            method: "POST",
            data: getPhotoAsFormData(file),
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).
       success(function (data, status, headers, config) {
           debugger;
           $scope.newProduct.Photo = data;
           $scope.newProduct.PhotoUrl = "/UploadPhotos/" + data;
          
       }).
       error(function (data, status, headers, config) {
           alert("failed!");
       });
    };
}