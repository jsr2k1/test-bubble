// -------------------------------------------------------
//  Created by Andrew Witte.
//  Copyright (c) 2013 Reign-Studios. All rights reserved.
// -------------------------------------------------------

#import "DFP_InterstitialAdNative.h"
#import "UnityTypes.h"

@implementation DFP_InterstitialAdNative
- (id)init
{
    self = [super init];
    // init other data here...
    events = [[NSMutableArray alloc] init];
    return self;
}

- (void)viewDidLoad
{
    // do nothing...
}

- (void)dealloc
{
    if (ad != nil)
    {
        ad.delegate = nil;
        [ad release];
        ad = nil;
    }
    
    if (unitID != nil)
    {
        delete[] unitID;
        unitID = nil;
    }
    
    [events dealloc];
    [super dealloc];
}

- (void)interstitialDidReceiveAd:(DFPInterstitial*)interstitial
{
    [events addObject:@"Cached"];
}

- (void)interstitial:(DFPInterstitial*)interstitial didFailToReceiveAdWithError:(GADRequestError*)error
{
    [events addObject:[NSString stringWithFormat:@"Error:%@", [error localizedDescription]]];
    NSLog(@"%@",[error localizedDescription]);
}

- (void)interstitialWillPresentScreen:(DFPInterstitial*)interstitial
{
    [events addObject:@"Shown"];
}

- (void)interstitialWillDismissScreen:(DFPInterstitial*)interstitial
{
    [events addObject:@"Canceled"];
}

- (void)interstitialDidDismissScreen:(DFPInterstitial*)interstitial
{
    // do nothing...
}

- (void)interstitialWillLeaveApplication:(DFPInterstitial*)interstitial
{
    [events addObject:@"Clicked"];
}

- (void)CreateAd:(char*)unitIDasci
{
    unitID = new char[strlen(unitIDasci) + 1];
    strcpy(unitID, unitIDasci);
}

- (void)Cache
{
    // delete used ad
    if (ad != nil && ad.hasBeenUsed)
    {
        ad.delegate = nil;
        [ad release];
        ad = nil;
    }
    
    // create new ad
    ad = [[DFPInterstitial alloc] init];
    ad.adUnitID = [[NSString alloc] initWithUTF8String:unitID];
    ad.delegate = self;
    
    if (testing)
    {
        GADRequest *request = [GADRequest request];
        request.testing = YES;
        request.testDevices = [NSArray arrayWithObjects:GAD_SIMULATOR_ID, nil];
        [ad loadRequest:request];
    }
    else
    {
        [ad loadRequest:[GADRequest request]];
    }
}

- (void)Show
{
    UIViewController* view = UnityGetGLViewController();
    [ad presentFromRootViewController:view];
    
}
@end

// ----------------------------------
// Unity C Link
// ----------------------------------
extern "C"
{
    DFP_InterstitialAdNative* DFP_Interstitial_InitAd(bool testing)
    {
        DFP_InterstitialAdNative* ad = [[DFP_InterstitialAdNative alloc] init];
        ad->testing = testing;
        return ad;
    }
    
    void DFP_Interstitial_DisposeAd(DFP_InterstitialAdNative* ad)
    {
        if (ad != nil) [ad release];
    }
    
    void DFP_Interstitial_CreateAd(DFP_InterstitialAdNative* ad, char* unitID)
    {
        [ad CreateAd:unitID];
    }
    
    void DFP_Interstitial_Cache(DFP_InterstitialAdNative* ad)
    {
        [ad Cache];
    }
    
    void DFP_Interstitial_Show(DFP_InterstitialAdNative* ad)
    {
        [ad Show];
    }
    
    bool DFP_Interstitial_AdHasEvents(DFP_InterstitialAdNative* ad)
    {
        return ad->events.count != 0;
    }
    
    const char* DFP_Interstitial_GetNextAdEvent(DFP_InterstitialAdNative* ad)
    {
        const char* ptr = [[ad->events objectAtIndex:0] cStringUsingEncoding:NSUTF8StringEncoding];
        [ad->events removeObjectAtIndex:0];
        return ptr;
    }
}