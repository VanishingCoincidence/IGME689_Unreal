// Fill out your copyright notice in the Description page of Project Settings.


#include "FeatureLayerQuery.h"

#include <rapidjson/reader.h>

// Sets default values
AFeatureLayerQuery::AFeatureLayerQuery()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AFeatureLayerQuery::BeginPlay()
{
	Super::BeginPlay();

	ProcessRequest();
	
}

// Called every frame
void AFeatureLayerQuery::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AFeatureLayerQuery::OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bSucceeded)
{
	if (!bSucceeded)
	{
		return;
	}

	TSharedPtr<FJsonObject> responseObject;
	const auto ResponseBody = Response-> GetContentAsString();
	auto Reader = TJsonReaderFactory<>::Create(ResponseBody);

	UE_LOG(LogTemp, Warning, TEXT("%s"), *ResponseBody);

	if (FJsonSerializer::Deserialize(Reader, responseObject))
	{
		auto allFeatures = responseObject->GetArrayField(TEXT("features"));

		for(auto feature : allFeatures)
		{
			FProperties currentFeature;
			auto coordinates = feature->AsObject()->GetObjectField(TEXT("geometry"))->GetArrayField(TEXT("coordinates"));
            auto name = feature->AsObject()->GetObjectField(TEXT("properties"))->GetStringField("Name");
            auto location = feature->AsObject()->GetObjectField(TEXT("properties"))->GetStringField("location");
            auto altitude = feature->AsObject()->GetObjectField(TEXT("properties"))->GetIntegerField("altitude");
            auto length = feature->AsObject()->GetObjectField(TEXT("properties"))->GetIntegerField("length");
            
            currentFeature.Name = name;
            currentFeature.Altitude = altitude;
            currentFeature.Location = location;
            currentFeature.Length = length;
            
            for (int i = 0; i < coordinates.Num(); i++)
            {
            	auto thisGeometry = coordinates[i]->AsArray();
            	FGeometries geometry;
            	geometry.Geometry.Add(thisGeometry[0]->AsNumber());
            	geometry.Geometry.Add(thisGeometry[1]->AsNumber());
            	currentFeature.Geometries.Add(geometry);
            }
            
            features.Add(currentFeature);
		}
	}
}

void AFeatureLayerQuery::ProcessRequest()
{
	FHttpRequestRef Request = FHttpModule::Get().CreateRequest();
	Request->OnProcessRequestComplete().BindUObject(this, &AFeatureLayerQuery::OnResponseReceived);
	Request->SetURL(webLink);
	Request->SetVerb("Get");
	Request->ProcessRequest();
}

