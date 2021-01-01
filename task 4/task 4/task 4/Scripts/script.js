$('#search').keyup(function () {
    //Url for the Cloud image hosting
    var urlForCloudImage = "http://res.cloudinary.com/doh5kivfn/image/upload/v1460006156/talents/";

    var urlForJson = "https://localhost:44334/api/v1/talents";

    var searchField = $('#search').val();
    var myExp = new RegExp(searchField, "i");

    $.getJSON(urlForJson, function (data) {
        var output = '<ul class="searchresults">';
        $.each(data, function (key, val) {
            //for debug
            console.log(data);
            if ((val.Name.search(myExp) != -1) ||
                (val.Bio.search(myExp) != -1)) {
                output += '<li>';
                output += '<h2>' + val.Name + '</h2>';
                //get the absolute path for local image
                //output += '<img src="images/'+ val.ShortName +'_tn.jpg" alt="'+ val.Name +'" />';

                //get the image from cloud hosting
                output += '<img src=' + urlForCloudImage + val.ShortName + "_tn.jpg alt=" + val.Name + '" />';
                output += '<p>' + val.Bio + '</p>';
                output += '</li>';
            }
        });
        output += '</ul>';
        $('#update').html(output);
    }); //get JSON

});
