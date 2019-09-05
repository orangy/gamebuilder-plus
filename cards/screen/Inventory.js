export const PROPS = [
  propNumber('X', 50, { label: "Center X (0-100%):"}),
  propNumber('Y', 50, { label: "Center Y (0-100%):"}),
  propNumber('W', 35, { label: "Width (0-100%):"}),
  propNumber('H', 20, { label: "Height X (0-100%):"}),

  propBoolean("CustomBox", false, {label: "Customize Box:"}), 
    propColor('BoxColor', '#999999', { requires: [requireTrue("CustomBox")] }),
    propColor('BoxBorderColor', '#222222', { requires: [requireTrue("CustomBox")] }),
  
  propBoolean("CustomText", false, {label: "Customize Text:"}),
    propNumber('TextSize', 20, { requires: [requireTrue("CustomText")] }),
    propColor('TextColor', '#000000', { requires: [requireTrue("CustomText")] }),
    propString('Text', 'Hello world!', { requires: [requireTrue("CustomText")] }),
  propBoolean("CustomItemBox", false, {label: "Customize Item Box:"}),
    propDecimal('ItemBoxSize', 2, { requires: [requireTrue("CustomItemBox")] }),
    propDecimal('ItemBoxTextSize', 10, { requires: [requireTrue("CustomItemBox")] }),

  propNumber('UnknownItemX', 0,),
  propNumber('UnknownItemY', 0,),
  propBoolean("SpriteFilter", false, { label: "Sprite Filter" }),
  propImage("Sprite", { label: "Sprite Upload" }),
  propNumber('SpriteWidth', 34),
  propNumber('SpriteHeight', 34)
]
 function uiSpriteXY(xy,wh,indexXY) {
  uiImageSlice(xy[0], xy[1], wh[0], wh[1], indexXY[0]*props.SpriteWidth, indexXY[1]*props.SpriteHeight, props.SpriteWidth, props.SpriteHeight, props.Sprite, {noFilter: !props.SpriteFilter });
 }

export function onDrawScreen() {
  const player = getControllingPlayer();
  if (player && player !== getLocalPlayer()) {
    return;
  }

  //card.h = invIndex*props.ItemBoxSize*1.1*(inventory.length/mod[0])*uiGetScreenRatio()+invIndex;
  let wh = screenPercentage([props.W,props.H]);
  let xy = screenPercentage([props.X,props.Y]);
  let boxXY = [xy[0]-wh[0]*0.5,xy[1]-wh[1]*0.5];
  uiRect(boxXY[0],boxXY[1],wh[0],wh[1],props.BoxColor,{opacity: props.Opacity});
  uiRect(boxXY[0],boxXY[1],wh[0],wh[1],props.BoxBorderColor,{opacity: props.Opacity,style: "BORDER"});
  uiText(xy[0]-0.5*uiGetTextWidth(props.Text,props.TextSize),xy[1]-wh[1]*0.475, props.Text, props.TextColor,{textSize: props.TextSize});
  if(typeof card.showInventory != "undefined" && card.showInventory != null && card.showInventory.length != null && card.showInventory.length > 0)
    card.showInventory.forEach(displayInventory);
  if(!(!card.pickUp || /^\s*$/.test(card.pickUp)))
    uiSpriteXY([getMouseX(),uiGetScreenHeight()-getMouseY()],screenPercentage([props.ItemBoxSize,props.ItemBoxSize*uiGetScreenRatio()]),getSpriteXY(getItem(card.pickUp)[2]));
  cooldown(0.02)
}

export function onInit(){
  card.showInventory =[["5/10:1,1:Carrot","1/1:0,12:Shield","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","",""],["5/10:1,1:Carrot","1/1:0,12:Shield","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","",""]];
  card.pickUp = "";
}

export function onPushInventory(arg) {
  card.showInventory.push(arg.inventory);
}

export function onUpdateInventory(arg) {
  card.showInventory[arg.index] = arg.inventory;
}

function screenPercentage(xy) {
  return [xy[0]*0.01*uiGetScreenWidth(),xy[1]*0.01*uiGetScreenHeight()];
}

function displayInventory(inventory,invIndex,inventories)
{
  const mod = [Math.floor(props.W/(props.ItemBoxSize * 1.1)),Math.floor(props.H/(props.ItemBoxSize * 1.1))];
  const offset = invIndex*props.ItemBoxSize*1.1*(inventory.length/mod[0])*uiGetScreenRatio()+invIndex;

  let index = 0;
  for(index=0;index<inventory.length;index++) {
    const item = getItem(inventory[index]);
    const itemQuantityInfo = getItemQuantityInfo(item[1]);
    const wh = screenPercentage([props.ItemBoxSize,props.ItemBoxSize*uiGetScreenRatio()]);
    const xy = screenPercentage([props.X-props.W*0.4675+props.ItemBoxSize*1.1*Math.floor(index % mod[0]),props.Y-props.H*0.4+props.ItemBoxSize*1.1*uiGetScreenRatio()*Math.floor(index/mod[0] % mod[1])+offset]); 
    //card.showInventoryCoords[invIndex][index] = [xy[0],xy[1],xy[0]+wh[0],xy[1]+wh[1]];
    const indexXY = getSpriteXY(item[2]);
    uiRect(xy[0],xy[1],wh[0],wh[0],props.BoxBorderColor,{opacity: props.Opacity, style: "FILLED"});
    uiRect(xy[0],xy[1],wh[0],wh[0],props.TextColor,{opacity: props.Opacity,style: "BORDER"});
    if(item[3] !== "Empty") uiSpriteXY(xy,[wh[0],wh[0]],indexXY);
    uiTextShadow(xy[0]+wh[0]*0.05,xy[1]+wh[0]*0.7,itemQuantityInfo[0],UiColor.WHITE,{ textSize: props.ItemBoxTextSize});
    uiText(xy[0]+wh[0]*0.05,xy[1]+wh[0]*0.7,itemQuantityInfo[0],UiColor.WHITE,{ textSize: props.ItemBoxTextSize});
    uiTextShadow(xy[0]+wh[0]*0.05+uiGetTextWidth(itemQuantityInfo[0],props.ItemBoxTextSize)*1.3,xy[1]+wh[0]*0.7,item[3],UiColor.WHITE,{ textSize: props.ItemBoxTextSize});
    uiText(xy[0]+wh[0]*0.05+uiGetTextWidth(itemQuantityInfo[0],props.ItemBoxTextSize)*1.3,xy[1]+wh[0]*0.7,item[3],UiColor.WHITE,{ textSize: props.ItemBoxTextSize});
    uiButton(xy[0],xy[1],wh[0],wh[0],"",'ItemClick',{ clickMessageArg: { invIndex: invIndex, index: index, item: item}, opacity: 0 });
  }
}

function uiTextShadow(x,y,text,color,options,shadowXY) {
  if(shadowXY === undefined ) shadowXY = [1,1];
  uiText(x+shadowXY[0],y+shadowXY[1],text,UiColor.BLACK,options);
  return true;
}

function getItem(item) { //Returns [str,count,spriteXY,item]
  if(!item || /^\s*$/.test(item)) return ["","",",","Empty"]
  const regex = /([0-9+\/]*):(.*):(.*)/;
  return regex.exec(item);
}

export function onSecondaryAction() {
  const mouseXY = [getMouseX(),uiGetScreenHeight()-getMouseY()];
  let invIndex = 0;
  for(invIndex=0;index<card.showInventory.length;invIndex++) {
    let index = 0;
    for(index=0;index<card.showInventory[invIndex].length;index++) 
      if(mouseXY[0] >= card.showInventoryCoords[invIndex][index][0] && mouseXY[0] <= card.showInventoryCoords[invIndex][index][2])
        if(mouseXY[1] >= card.showInventoryCoords[invIndex][index][1] && mouseXY[1] <= card.showInventoryCoords[invIndex][index][3]) {
          log(index);
          return;
        }
  }
}

export function onItemClick(arg) {

  const temp = card.pickUp;
  card.pickUp = card.showInventory[arg.invIndex][arg.index];
  card.showInventory[arg.invIndex][arg.index] = temp;
  //send(card.inventoryActor[0],'UpdateInventory',{ index: 0, inventory: card.showInventory[0]})
}

function uiGetScreenRatio(inv) {
  if(inv) return uiGetScreenHeight()/uiGetScreenWidth();
  else return uiGetScreenWidth()/uiGetScreenHeight();
}

function getSpriteXY(str) {  return /([0-9]*),([0-9]*)/.exec(str).splice(1); }
function getItemQuantityInfo(str) {  
  if(!str || /^\s*$/.test(str)) return ["",""];
  return /([0-9]*)\/([0-9]*)/.exec(str).splice(1);
}