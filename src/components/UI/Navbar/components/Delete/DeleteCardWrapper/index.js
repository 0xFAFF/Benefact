import React from "react";
import PropTypes from "prop-types";
import { Confirm, Back } from "../../../../Popup";
import Preview from "../../../../MarkdownEditor";
import "./index.scss";

class DeleteCardWrapper extends React.Component {
  static propTypes = {
    cards: PropTypes.array,
    handleUpdate: PropTypes.func,
    onClose: PropTypes.func
  };

  state = {
    card: null
  };

  onAcceptHandler = () => {
    this.props.handleUpdate("cards", "DELETE", { id: this.state.card.id });
    this.props.onClose();
  };

  onCancelHandler = () => {
    this.setState({ card: null });
  };

  render() {
    const { cards } = this.props;
    return (
      <div id="delete-card-outer">
        {!this.state.card ? (
          <>
            <Back
              title={"Back"}
              onClick={() => this.props.resetDeleteComponentHandler()}
            />
            <div className="title">Cards</div>
            <div className="delete-button-container">
              <div className="delete-button-group">
                {cards.map(card => {
                  return (
                    <button
                      className="delete-button"
                      key={card.id}
                      onClick={() => this.setState({ card })}
                    >
                      {card.title}
                    </button>
                  );
                })}
              </div>
            </div>
          </>
        ) : (
          <Confirm
            onAcceptHandler={this.onAcceptHandler}
            onCancelHandler={this.onCancelHandler}
          >
            <Back title={"Back"} onClick={() => this.onCancelHandler()} />
            <div id="selected-card">
              <Preview content={this.state.card} />
            </div>
          </Confirm>
        )}
      </div>
    );
  }
}
export default DeleteCardWrapper;
