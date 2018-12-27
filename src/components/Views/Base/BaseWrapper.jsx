import React from "react";
import { get } from "lodash";
import { getCards, camelCase, getFetching, fetching } from "../../../utils";
import { URLS } from "../../../constants";
import { PacmanLoader } from "../../UI/Loader";
import Base from "./Base";
import { Navbar } from "../../UI";
import { TagsProvider } from "../../UI/BoardComponents/Tags/TagsContext";

// const url = "http://192.168.1.4/api/cards";
const url = "http://benefact.faffgames.com/api/cards";

class BaseWrapper extends React.Component {
  state = {
    view: "kanban",
    cards: [],
    columns: [],
    tags: [],
    columnOrder: [],
    error: null
  };

  handleResetState = data => {
    let columnOrder = data.columns.map(column => column.id);
    this.setState({
      cards: data.cards,
      columns: data.columns,
      tags: data.tags,
      columnOrder: columnOrder
    });
  };

  componentDidUpdate(prevProps) {
    if (!prevProps.data && this.props.data) {
      let formattedData = camelCase(this.props.data);
      this.handleResetState(formattedData);
    }
  }

  handleUpdate = async (type, action, method, newContent, newState) => {
    const url = URLS(type, action);
    await fetching(url, method, newContent).then(result => {
      if (result.hasError) {
        this.handleError(result.message);
      } else {
        this.setState(newState);
      }
    });
  };

  handleOrder = async (type, newContent) => {
    const url = URLS(type, "UPDATE");
    await fetching(url, "POST", newContent)
      .then(result => {
        if (result.hasError) {
          this.handleError(result.message);
        }
      })
      .then(async result => {
        const url = URLS(type, "GET");
        await fetching(url, "GET").then(result => {
          let formattedData = camelCase(result.data);
          this.handleResetState(formattedData);
        });
      });
  };

  kanbanOnDragEnd = result => {
    const { destination, source, draggableId, type } = result;
    // check if there is a destination
    if (!destination) return;

    // check if droppable has moved
    if (
      destination.droppableId === source.droppableId &&
      destination.index === source.index
    ) {
      return;
    }

    // Move columns around
    if (type === "column") {
      const newColumnOrder = Array.from(this.state.columnOrder);
      newColumnOrder.splice(source.index, 1);
      newColumnOrder.splice(destination.index, 0, draggableId);

      const newState = {
        ...this.state,
        columnOrder: newColumnOrder
      };
      this.setState(newState);
      return;
    }

    // source column of droppable
    const start = this.state.columns.find(
      column => `col-${column.id}` === source.droppableId
    );
    // destination column of droppable
    const finish = this.state.columns.find(
      column => `col-${column.id}` === destination.droppableId
    );

    // Moving within one column
    if (start === finish) {
      // new cards nonmutated array
      let cardsSrcCol = getCards(this.state.cards, source.droppableId, "col-");
      let cardsNotSrcDestCols = this.state.cards.filter(
        card =>
          `col-${card.columnId}` !== source.droppableId &&
          `col-${card.columnId}` !== destination.droppableId
      );
      const draggedCard = cardsSrcCol.find(
        card => `card-${card.id}` === draggableId
      );

      let newIndex = cardsSrcCol[destination.index]["index"];
      draggedCard["index"] = newIndex;
      // Orders array for inserting droppable in new spot
      cardsSrcCol.splice(source.index, 1);
      cardsSrcCol.splice(destination.index, 0, draggedCard);
      const newState = {
        ...this.state,
        cards: [...cardsSrcCol, ...cardsNotSrcDestCols]
      };
      this.setState(newState);
      this.handleOrder("cards", draggedCard);
      return;
    }

    // Moving card from one column to another
    let cardsSrcCol = getCards(this.state.cards, source.droppableId, "col-");
    let cardsDestCol = getCards(
      this.state.cards,
      destination.droppableId,
      "col-"
    );
    let cardsNotSrcDestCols = this.state.cards.filter(
      card =>
        `col-${card.columnId}` !== source.droppableId &&
        `col-${card.columnId}` !== destination.droppableId
    );
    const draggedCard = cardsSrcCol.find(
      card => `card-${card.id}` === draggableId
    );

    // convert col-# string into integer #
    draggedCard.columnId = +destination.droppableId.replace(/^\D+/g, "");
    let destCard = get(cardsDestCol, `[${destination.index}]`, null);
    if (destCard) {
    } else {
      draggedCard["index"] = cardsDestCol.length;
    }

    cardsSrcCol.splice(source.index, 1);
    cardsDestCol.splice(destination.index, 0, draggedCard);

    const newState = {
      ...this.state,
      cards: [...cardsSrcCol, ...cardsDestCol, ...cardsNotSrcDestCols]
    };
    this.setState(newState);
    this.handleOrder("cards", draggedCard);
  };

  listOnDragEnd = result => {
    const { destination, source, draggableId } = result;
    // check if there is a destination
    if (!destination) return;
    // check if droppable has moved
    if (
      destination.droppableId === source.droppableId &&
      destination.index === source.index
    ) {
      return;
    }

    let cards = [...this.state.cards];
    const draggedCard = cards.find(card => `card-${card.id}` === draggableId);
    // Orders array for inserting droppable in new spot
    cards.splice(source.index, 1);
    cards.splice(destination.index, 0, draggedCard);
    const newState = {
      ...this.state,
      cards: [...cards]
    };
    this.setState(newState);
    return;
    // Call endpoint here to API endpoint to connect to backend as well
  };

  updateBoardContent = (newContent, type) => {
    let boardType = [...this.state[type]];
    boardType[
      boardType.findIndex(type => type.id === newContent.id)
    ] = newContent;
    const newState = {
      ...this.state,
      [type]: boardType
    };
    this.handleUpdate(type, "UPDATE", "POST", newContent, newState);
  };

  addNewCard = (newContent, columnId) => {
    const newCard = {
      title: newContent.title || "",
      description: newContent.description || "",
      tagIds: newContent.tagIds || [],
      columnId: columnId
    };
    const newState = {
      ...this.state,
      cards: [...this.state.cards, { id: newContent.id, ...newCard }]
    };
    this.handleUpdate("cards", "ADD", "POST", newCard, newState);
  };

  addNewColumn = title => {
    const newId = this.state.columns.length + 1;
    const newColumn = {
      id: newId,
      title
    };
    const newState = {
      ...this.state,
      columns: [...this.state.columns, newColumn],
      columnOrder: [...this.state.columnOrder, newId]
    };
    this.handleUpdate("columns", "ADD", "POST", newColumn, newState);
  };

  addNewTag = tag => {
    const { name, color = null, character = null } = tag;
    const newId = this.state.tags.length + 1;
    const newTag = {
      id: newId,
      name,
      color,
      character
    };
    const newState = {
      ...this.state,
      tags: [...this.state.tags, newTag]
    };
    this.handleUpdate("tags", "ADD", "POST", newTag, newState);
  };

  changeBoardView = view => {
    this.setState({ view });
  };

  handleError = message => {
    this.setState({
      error: message
    });
  };

  componentDidMount() {
    this.setState({ error: this.props.error });
  }

  render() {
    const { cards } = this.state;
    const { isLoading } = this.props;
    const { error = this.props.error, ...baseState } = this.state;

    const kanbanFunctions = {
      kanbanOnDragEnd: this.kanbanOnDragEnd,
      updateBoardContent: this.updateBoardContent,
      addNewCard: this.addNewCard,
      // addNewColumn: this.addNewColumn,
      addNewTag: this.addNewTag
    };

    const listFunctions = {
      listOnDragEnd: this.listOnDragEnd,
      updateBoardContent: this.updateBoardContent,
      addNewCard: this.addNewCard
    };

    if (error) {
      return <div>Error: {error}</div>;
    } else if (isLoading || cards.length === 0) {
      return <PacmanLoader />;
    }
    return (
      <TagsProvider value={this.state.tags}>
        <div>
          <Navbar
            handleBoardView={this.changeBoardView}
            view={this.state.view}
            addNewColumn={this.addNewColumn}
            addNewTag={this.addNewTag}
            addNewCard={this.addNewCard}
            cardMap={this.state.cards}
          />
          <Base
            {...baseState}
            handleError={this.handleError}
            kanbanFunctions={kanbanFunctions}
            listFunctions={listFunctions}
          />
        </div>
      </TagsProvider>
    );
  }
}

export default getFetching(url)(BaseWrapper);
