// Input event
registerInputEvent(fxDTSBrick, "onBotRelay", "Self fxDTSBrick" TAB "Bot Bot");
function fxDTSBrick::onBotRelay(%obj)
{
  $InputTarget_["Self"] = %obj;
  $InputTarget_["Bot"] = %obj.hBot;

  %obj.processInputEvent("onBotRelay");
}

// Output event
registerOutputEvent(fxDTSBrick, "fireBotRelay");
function fxDTSBrick::fireBotRelay(%obj)
{
  // Use the same relay quota as the default fireRelay event.
  %currTime = getSimTime();
  if(%currTime - %obj.lastRelayTime < 15)
  {
    return;
  }
  %obj.lastRelayTime = %currTime;

  %obj.onBotRelay();
}

// Package
package BotRelayEvents
{
  function serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4)
  {
    // Force a delay of 33ms.
    Parent::serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
    %brick = %client.wrenchBrick;

    %i = %brick.numEvents-1;
    if(%brick.eventOutput[%i] $= "fireBotRelay")
    {
      if(%brick.eventDelay[%i] < 33)
      {
        %brick.eventDelay[%i] = 33;
      }
    }
  }
};

deactivatePackage("BotRelayEvents");
activatePackage("BotRelayEvents");
