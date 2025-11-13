// Fill out your copyright notice in the Description page of Project Settings.


#include "FeatureLayerQuery2.h"

// Sets default values
AFeatureLayerQuery2::AFeatureLayerQuery2()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AFeatureLayerQuery2::BeginPlay()
{
	Super::BeginPlay();

	ProcessRequest();
	
}

// Called every frame
void AFeatureLayerQuery2::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AFeatureLayerQuery2::OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bSucceeded)
{
	if (!bSucceeded)
	{
		return;
	}

	TSharedPtr<FJsonObject> responseObject;
	const auto ResponseBody = Response->GetContentAsString();
}

void AFeatureLayerQuery2::ProcessRequest()
{
	FHttpRequestRef Request = FHttpModule::Get().CreateRequest();
	Request->OnProcessRequestComplete().BindUObject(this, &AFeatureLayerQuery2::OnResponseReceived);
	Request->SetURL(webLink);
	Request->SetVerb("Get");
	Request->ProcessRequest();
}

