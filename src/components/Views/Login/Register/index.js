import React from "react";
import PropTypes from "prop-types";
import { URLS } from "../../../../constants";
import { fetching } from "../../../../utils";
import { Create, Login } from "./components";
import "./index.scss";

class Register extends React.Component {
  state = {
    fields: {
      email: "",
      username: "",
      password: "",
      confirmPassword: ""
    },
    registered: false,
    token: ""
  };

  onInputChangeHandler = (e, field) => {
    this.setState({
      fields: {
        ...this.state.fields,
        [field]: e.target.value
      }
    });
  };

  onCreateAccount = async () => {
    const {
      fields: { email, username, password, confirmPassword }
    } = this.state;
    if (!email || !username || !password || !confirmPassword) {
      console.warn("There's an empty field");
      console.log(this.state);
      return;
    }

    // Validate password and confirmPassword
    if (password === confirmPassword) {
      const url = URLS("users", "ADD");
      const queryParams = {
        email: email,
        name: username,
        password: password
      };
      await fetching(url, "POST", queryParams)
        .then(result => {
          if (result.hasError) {
            this.handleError(result.message);
          } else {
            this.setState({ registered: true });
          }
        })
        .then(async result => {
          const url = URLS("users", "GET");
          const queryParams = {
            email: email,
            password: password
          };
          await fetching(url, "POST", queryParams).then(result => {
            if (result.hasError) {
              this.handleError(result.message);
            } else {
              this.setState({ token: result.data });
            }
          });
        });
    } else {
      console.warn("password and confirmPassword are different");
    }
  };

  render() {
    const { onViewChangeHandler, onLoginHandler } = this.props;
    const { fields, registered, token } = this.state;
    return (
      <div id="register-container">
        {!registered && (
          <Create
            fields={fields}
            onViewChangeHandler={onViewChangeHandler}
            onInputChangeHandler={this.onInputChangeHandler}
            onCreateAccount={this.onCreateAccount}
          />
        )}
        {registered && <Login token={token} onLoginHandler={onLoginHandler} />}
      </div>
    );
  }
}

Register.propTypes = {
  onViewChangeHandler: PropTypes.func
};

export default Register;
