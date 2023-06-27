
function postAjax(routeString, params, callback) {
    var xhttp = new XMLHttpRequest();
    let parameters = JSON.stringify(params);

    xhttp.open("POST", routeString, true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

    xhttp.onload = function () {
        callback(this);
    };
    xhttp.send(parameters);
}

function postAjaxFormData(action, formData, success, error) {
    var xhttp = new XMLHttpRequest();

    xhttp.open("POST", action, true);

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4) {
            let responseData;
            try {
                responseData = JSON.parse(this.response);
            }
            catch {
                responseData = this.response;
            }

            if (this.status >= 100 && this.status < 400) {
                success(responseData);
            } else if (error) {
                error(responseData);
            } else {
                success(responseData);
            }
        }
    };
    xhttp.send(formData);
}

function postAjaxForm(formElement, success, error) {
    const formData = new FormData(formElement);

    postAjaxFormData(formElement.action, formData, success, error);
}