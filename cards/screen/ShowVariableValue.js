export const PROPS = [
  propCardTargetActor("Target", {
    label: "Whose variable?"
  }),
  propNumber('X', 100),
  propNumber('Y', 100),
  propString('ColorHex', '#ffff00'),
  propString('Text', 'Gold: #vargold'),
]

export function onDrawScreen() {
  card.text = props.Text.replace(/#var([a-zA-Z]*)/g,varReplace);
  uiText(props.X, props.Y, card.text, colorFromHex(props.ColorHex) || UiColor.WHITE);
}

function varReplace(match, variable) {
  return getVar(variable, getCardTargetActor("Target")) || 0;
}